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
        private readonly IPasswordHasher<AppUser> _hasher;
        private readonly IEmailService _emailService;
        private readonly JwtOptions _jwtOptions;
        private readonly ClaimsPrincipal? _user;
        private readonly IHostEnvironment _host;
        private readonly IUploadService _uploadService;

        public UserService(IRepositoryManager repository, IMapper mapper,
            IPasswordHasher<AppUser> hasher, IOptions<JwtOptions> jwt,
            IHttpContextAccessor contextAccessor,
            IEmailService emailService, IHostEnvironment host,
            IUploadService uploadService)
        {
            _repository = repository;
            _mapper = mapper;
            _hasher = hasher;
            _emailService = emailService;
            _jwtOptions = jwt.Value;
            _user = contextAccessor.HttpContext?.User;
            _host = host;
            _uploadService = uploadService;
        }

        public async Task<ApiResult<UserInfoDto>> GetLoggedInUser()
        {
            var loggedInUserEmail = _user?.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrWhiteSpace(loggedInUserEmail))
            {
                return new ApiResult<UserInfoDto>("Access denied. User not logged in.", 403);
            }

            var loggedInUser = await _repository.User.GetByEmailAsync(loggedInUserEmail);
            if(loggedInUser == null)
            {
                return new ApiResult<UserInfoDto>("User record not found", 404);
            }

            return new ApiResult<UserInfoDto>(_mapper.Map<UserInfoDto>(loggedInUser));
        }

        public async Task<UserInfoDto> RegisterAsync(RegisterUserCommand command)
        {
            var existingUser = await _repository.User.GetByEmailAsync(command.EmailAddress);
            if(existingUser != null)
            {
                return null!;
            }

            var appUser = _mapper.Map<AppUser>(command);
            appUser.PasswordHash = _hasher.HashPassword(appUser, command.Password);
            await _repository.User.AddAsync(appUser, false);
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
            await _emailService.SendAsync(appUser.Email, message, "Verify Your Account");
            return _mapper.Map<UserInfoDto>(appUser);
        }

        public async Task<ApiResult<TokenDto>> LoginAsync(LoginCommand command)
        {
            var user = await _repository.User.GetByEmailAsync(command.Email);
            if(user == null)
            {
                return new ApiResult<TokenDto>("No user found with the specified email address", 404);
            }

            if(user.Status != Domain.Enums.Statuses.Active)
            {
                return new ApiResult<TokenDto>("You can't login right now. Account is not active yet.", 403);
            }

            var loginResult = _hasher.VerifyHashedPassword(user, user.PasswordHash, command.Password);

            if (loginResult == PasswordVerificationResult.Failed)
            {
                return new ApiResult<TokenDto>("Incorrect password", 400);
            }

            var jwtToken = GenerateAccessToken(user);
            return new ApiResult<TokenDto>(new TokenDto
            {
                AccessToken = jwtToken
            });
        }

        public async Task<ApiResult<string>> VerifyAccountAsync(AccountVerificationCommand command)
        {
            var user = await _repository.User.GetByEmailAsync(command.Email);
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
            await _repository.User.UpdateAsync(user);
            await _repository.User.DeleteOtp(otp);
            return new ApiResult<string>("Account verification successful. You can proceed to login");
        }

        public async Task<UserInfoDto?> GetById(Guid id)
        {
            var user = await _repository.User.GetByIdAsync(id);
            if(user == null)
            {
                return null;
            }
            return _mapper.Map<UserInfoDto>(user);
        }

        public async Task<UserInfoDtoV2?> GetByIdV2Async(Guid id)
        {
            var user = await _repository.User.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            return _mapper.Map<UserInfoDtoV2>(user);
        }

        public List<UserInfoDto> GetAll(GetAllUsersQuery query)
        {
            var users = _repository.User.GetAll();
            if (!string.IsNullOrEmpty(query.Search))
            {
                users = users.Where(u => u.FirstName.Contains(query.Search, StringComparison.OrdinalIgnoreCase) || 
                u.LastName.Contains(query.Search, StringComparison.OrdinalIgnoreCase));
            }
            if (query.Role.HasValue)
            {
                users = users.Where(u => u.Role.Equals(query.Role.Value.ToString(), StringComparison.OrdinalIgnoreCase));
            }

            return _mapper.Map<List<UserInfoDto>>(users);
        }

        public async Task<UserInfoDto?> Update(UserUpdateCommand command)
        {
            var userId = _user.GetLoggedInUserId();
            if(userId == Guid.Empty)
            {
                return null;
            }

            var existing = await _repository.User.GetByIdAsync(userId);
            if(existing == null)
            {
                return null;
            }

            existing.FirstName = command.FirstName;
            existing.LastName = command.LastName;
            existing.OtherName = command.OtherName;

            _mapper.Map(command, existing);
            await _repository.User.UpdateAsync(existing);

            return _mapper.Map<UserInfoDto>(existing);
        }

        public async Task Delete(Guid id)
        {
            var user = await _repository.User.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User is null");
            }

            await _repository.User.DeleteAsync(user);
        }

        public async Task<ApiResult<string>> UploadProfileImage(IFormFile file)
        {
            if(file.Length <= 0)
            {
                return new ApiResult<string>("Invalid file uploaded", 400);
            }

            if(file.Length > MaxResourceSize)
            {
                return new ApiResult<string>("Maximum file size is 2mb.", 400);
            }

            if (!allowedFileType.Any(f => file.FileName.EndsWith(f)))
            {
                return new ApiResult<string>("Invalid file type.", 400);
            }

            var email = _user?.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(email))
            {
                return new ApiResult<string>("Access denied.", 403);
            }

            var user = await _repository.User.GetByEmailAsync(email);
            if(user == null)
            {
                return new ApiResult<string>("User not found", 404);
            }

            using var stream = file.OpenReadStream();
            stream.Position = 0;
            var uploadResult = await _uploadService.UploadImageAsync(user.Id.ToString("N"), file.FileName, stream);
            if (!uploadResult.Success)
            {
                return new ApiResult<string>("Profile image upload failed", 400);
            }

            user.ProfilePicture = uploadResult.Url;
            user.ProfilePicturePublicId = uploadResult.PublicId;
            await _repository.User.UpdateAsync(user);

            return new ApiResult<string>("Profile picture successfully uploaded");
        }

        private string GenerateAccessToken(AppUser user)
        {
            //jwt: header, payload, signature
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Email),
                new(ClaimTypes.Role, user.Role),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

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
    }
}