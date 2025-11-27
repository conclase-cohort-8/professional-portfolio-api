using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProfessionalPortfolio.Application.Commands;
using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Application.DTOs;
using ProfessionalPortfolio.Application.Queries;
using ProfessionalPortfolio.Application.Services.Interfaces;
using ProfessionalPortfolio.Application.Settings;
using ProfessionalPortfolio.Domain.Entities;
using ProfessionalPortfolio.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProfessionalPortfolio.Application.Services
{
    public class UserService : IUserService
    {
        private const long MaxResourceSize = 2048000;
        private readonly List<string> allowedFileType = new List<string> { ".png", ".jpeg", ".jpg" };
        private readonly List<string> allowedDocType = new List<string> { ".pdf", ".docx", ".doc" };


        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly JwtOptions _jwtOptions;
        private readonly ClaimsPrincipal? _user;
        private readonly IHostEnvironment _host;
        private readonly IUploadService _uploadService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserService(IRepositoryManager repository, IMapper mapper,
            IOptions<JwtOptions> jwt,
            IHttpContextAccessor contextAccessor,
            IEmailService emailService, IHostEnvironment host,
            IUploadService uploadService,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _repository = repository;
            _mapper = mapper;
            _emailService = emailService;
            _jwtOptions = jwt.Value;
            _user = contextAccessor.HttpContext?.User;
            _host = host;
            _uploadService = uploadService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ApiResult<UserInfoDto>> GetLoggedInUser()
        {
            var loggedInUserEmail = _user?.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrWhiteSpace(loggedInUserEmail))
            {
                return new ApiResult<UserInfoDto>("Access denied. User not logged in.", 403);
            }

            var loggedInUser = await _userManager.FindByEmailAsync(loggedInUserEmail);
            if(loggedInUser == null)
            {
                return new ApiResult<UserInfoDto>("User record not found", 404);
            }

            return new ApiResult<UserInfoDto>(_mapper.Map<UserInfoDto>(loggedInUser));
        }

        public async Task<ApiResult<string>> RegisterAsync(RegisterUserCommand command)
        {
            var existingUser = await _userManager.FindByEmailAsync(command.EmailAddress);
            if(existingUser != null)
            {
                return new ApiResult<string>("There is a user already existing with this email", 409);
            }

            var appUser = _mapper.Map<AppUser>(command);
            appUser.UserName = command.EmailAddress;
            var createResult = await _userManager.CreateAsync(appUser, command.Password);
            if (!createResult.Succeeded)
            {
                return new ApiResult<string>(createResult.Errors.FirstOrDefault()?.Description ?? "Registreation failed");
            }

            var roleResult = await _userManager.AddToRoleAsync(appUser, Roles.User.ToString());
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(appUser);
                return new ApiResult<string>(roleResult.Errors.FirstOrDefault()?.Description ?? "Registreation failed");
            }

            //Generate OTP
            var otp = Extensions.GenerateOtp();
            var (Hash, Salt) = Extensions.HashOtp(otp);
            await _repository.User.InsertOtp(new OtpEntry
            {
                OtpHash = Hash,
                OtpSalt = Salt,
                UserId = appUser.Id,
                Type = Domain.Enums.OtpType.Verification
            }, false);

            await _repository.SaveAsync();
            var message = GetAccountVerificationMessage(appUser.FirstName, otp, 5);
            await _emailService.SendAsync(appUser.Email!, message, "Verify Your Account");
            return new ApiResult<string>("Registration successful. Please check your email for confirmation code");
        }

        public async Task<ApiResult<TokenDto>> LoginAsync(LoginCommand command)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);
            if(user == null)
            {
                return new ApiResult<TokenDto>("No user found with the specified email address", 404);
            }

            if(user.Status != Statuses.Active || !user.EmailConfirmed)
            {
                return new ApiResult<TokenDto>("You can't login right now. Account is not active yet.", 403);
            }

            var loginResult = await _signInManager.CheckPasswordSignInAsync(user, command.Password, lockoutOnFailure: true);
            if (!loginResult.Succeeded)
            {
                return new ApiResult<TokenDto>("Incorrect password", 400);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var jwtToken = GenerateAccessToken(user, [.. roles]);
            return new ApiResult<TokenDto>(new TokenDto
            {
                AccessToken = jwtToken
            });
        }

        public async Task<ApiResult<string>> VerifyAccountAsync(AccountVerificationCommand command)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);
            if(user == null)
            {
                return new ApiResult<string>("User not found", 404);
            }

            var otp = await _repository.User
                .GetOtpAsync(o => o.UserId == user.Id && o.Type == OtpType.Verification);
            if(otp == null)
            {
                return new ApiResult<string>("No verification OTP found for user", 404);
            }

            var isValid = Extensions.IsAValidOtp(command.Otp, otp.OtpSalt, otp.OtpHash, otp.Expires);
            if (!isValid)
            {
                return new ApiResult<string>("Invalid OTP", 400);
            }

            user.UpdatedOn = DateTime.UtcNow;
            user.Status = Statuses.Active;
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
            await _repository.User.DeleteOtp(otp);
            return new ApiResult<string>("Account verification successful. You can proceed to login");
        }

        public async Task<UserInfoDto?> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                return null;
            }
            return _mapper.Map<UserInfoDto>(user);
        }

        public async Task<UserInfoDtoV2?> GetByIdV2Async(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            return _mapper.Map<UserInfoDtoV2>(user);
        }

        public List<UserInfoDto> GetAll(GetAllUsersQuery query)
        {
            var users = _userManager.Users;
            if (!string.IsNullOrEmpty(query.Search))
            {
                users = users.Where(u => u.FirstName.Contains(query.Search, StringComparison.OrdinalIgnoreCase) || 
                u.LastName.Contains(query.Search, StringComparison.OrdinalIgnoreCase));
            }

            return _mapper.Map<List<UserInfoDto>>(users);
        }

        public async Task<ApiResult<string>> Update(UserUpdateCommand command)
        {
            var userId = _user.GetLoggedInUserId();
            if (!string.IsNullOrWhiteSpace(userId))
            {
                return new ApiResult<string>("You are not allowed to access this resources", 403);
            }

            var existing = await _userManager.FindByIdAsync(userId);
            if(existing == null)
            {
                return new ApiResult<string>("User not found", 404);
            }

            _mapper.Map(command, existing);
            await _userManager.UpdateAsync(existing);

            return new ApiResult<string>("Updated successfully");
        }

        public async Task<ApiResult<string>> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return new ApiResult<string>("User not found", 404);
            }

            await _userManager.DeleteAsync(user);
            return new ApiResult<string>("Successfully deleted");
        }

        public async Task<ApiResult<string>> UploadUserCv(IFormFile cv)
        {
            var validationResult = ValidateFile(cv, FileType.Docs);
            if (!validationResult.Success)
            {
                return validationResult;
            }

            var userId = _user.GetLoggedInUserId();
            if(!string.IsNullOrWhiteSpace(userId))
            {
                return new ApiResult<string>("You're not allowed to access this resources", 403);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ApiResult<string>("User record not found", 404);
            }

            using var stream = cv.OpenReadStream();
            stream.Position = 0;
            var uploadResult = await _uploadService.UploadRawAsync(user.Id.Replace("-", ""), cv.FileName, stream);
            if (!uploadResult.Success)
            {
                return new ApiResult<string>("CV upload failed. Please try again later", 500);
            }

            user.ResumePublicId = uploadResult.PublicId;
            user.ResumeUrl = uploadResult.Url;
            user.UpdatedOn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return new ApiResult<string>("CV upload successful.");
        }

        public async Task<ApiResult<string>> UploadProfileImage(IFormFile file)
        {
            var validationResult = ValidateFile(file, FileType.Image);
            if (!validationResult.Success)
            {
                return validationResult;
            }

            var email = _user?.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(email))
            {
                return new ApiResult<string>("Access denied.", 403);
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ApiResult<string>("User not found", 404);
            }

            using var stream = file.OpenReadStream();
            stream.Position = 0;
            var uploadResult = await _uploadService.UploadImageAsync(user.Id.Replace("-", ""), file.FileName, stream);
            if (!uploadResult.Success)
            {
                return new ApiResult<string>("Profile image upload failed", 400);
            }

            user.ProfilePicture = uploadResult.Url;
            user.ProfilePicturePublicId = uploadResult.PublicId;
            await _userManager.UpdateAsync(user);

            return new ApiResult<string>("Profile picture successfully uploaded");
        }

        private string GenerateAccessToken(AppUser user, List<string> roles)
        {
            //jwt: header, payload, signature
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Email!),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_jwtOptions.Expires),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GetAccountVerificationMessage(string firstName, string otp, int validty)
        {
            var path = Path.Combine(_host.ContentRootPath, "wwwroot", "templates", "account-verification.html");
            if (File.Exists(path))
            {
                var template = File.ReadAllText(path);
                return template.Replace("{{FirstName}}", firstName)
                    .Replace("{{OTP}}", otp)
                    .Replace("{{validity}}", validty.ToString());
            }

            throw new Exception($"Path not found: {path}");
        }

        private ApiResult<string> ValidateFile(IFormFile file, FileType type)
        {
            if (file.Length <= 0)
            {
                return new ApiResult<string>("Invalid file uploaded", 400);
            }

            if (file.Length > MaxResourceSize)
            {
                return new ApiResult<string>("Maximum file size is 2mb.", 400);
            }

            if (type == FileType.Image && !allowedFileType.Any(f => file.FileName.EndsWith(f)))
            {
                return new ApiResult<string>("Invalid file type.", 400);
            }
            if(type == FileType.Docs && !allowedDocType.Any(f => file.FileName.EndsWith(f)))
            {
                return new ApiResult<string>("Invalid doc type", 400);
            }

            return new ApiResult<string>("Valid");
        }
    }
}