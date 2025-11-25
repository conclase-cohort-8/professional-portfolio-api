using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Application.Services.Interfaces;
using ProfessionalPortfolio.Application.Users.Commands;
using ProfessionalPortfolio.Application.Users.Dtos;
using ProfessionalPortfolio.Application.Users.Queries;
using ProfessionalPortfolio.Domain.Entities;

namespace ProfessionalPortfolio.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserInfoDto> RegisterAsync(RegisterUserCommand command)
        {
            // 1. Validate the inputs (e.g, check if no existing user with the email)
            if(string.IsNullOrWhiteSpace(command.FirstName)  || string.IsNullOrWhiteSpace(command.LastName))
            {
                return null!;
            }

            var appUser = _mapper.Map<AppUser>(command);
            appUser.PasswordHash = _hasher.HashPassword(appUser, command.Password);
            var otp = Extensions.GenerateOtp();
            var (hash, salt) = Extensions.HashOtp(otp);
            await _repository.User.AddAsync(appUser);
            return _mapper.Map<UserInfoDto>(appUser);
        }

        public async Task<ApiResult<TokenDto>> LoginAsync(LoginCommand command)
        {
            var user = await _repository.User.GetByEmailAsync(command.Email);
            if(user == null)
            {
                return new ApiResult<TokenDto>("No user found with the specified email address", 404);
            }

            // 3. if the user is null, Create a new AppUser object
            var appUser = new AppUser
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.EmailAddress,
                OtherName = command.OtherName
            };

            // 3. Save it in the in-memory repository
            await _repository.AddAsync(appUser);
            // 4. Return the created user info dto:
            return new UserInfoDto(appUser);
        }

        public async Task<UserInfoDto?> GetById(Guid id)
        {
            var user = await _repository.GetByIdAsync(id);
            if(user == null)
            {
                return null;
            }
            return new UserInfoDto(user);
        }

        public List<UserInfoDto> GetAll(GetAllUsersQuery query)
        {
            var users = _repository.GetAll();
            if (!string.IsNullOrEmpty(query.Search))
            {
                users = users.Where(u => u.FirstName.Contains(query.Search, StringComparison.OrdinalIgnoreCase) || 
                u.LastName.Contains(query.Search, StringComparison.OrdinalIgnoreCase));
            }
            if (query.Role.HasValue)
            {
                users = users.Where(u => u.Role.Equals(query.Role.Value.ToString(), StringComparison.OrdinalIgnoreCase));
            }

            return users.Select(u => new UserInfoDto(u))
                .ToList();
        }

        public async Task<UserInfoDto?> Update(Guid id, UserUpdateCommand command)
        {
            if(command == null || string.IsNullOrEmpty(command.FirstName) || string.IsNullOrEmpty(command.LastName))
            {
                return null;
            }

            var existing = await _repository.GetByIdAsync(id);
            if(existing == null)
            {
                return null;
            }

            var cloneExisting = existing;

            existing.FirstName = command.FirstName;
            existing.LastName = command.LastName;
            existing.OtherName = command.OtherName;

            await _repository.DeleteAsync(cloneExisting);
            await _repository.UpdateAsync(existing);

            return new UserInfoDto(existing);
        }

        public async Task Delete(Guid id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User is null");
            }

        public void VerifyAsync()
        {
            //if (string.IsNullOrWhiteSpace(request.Otp) || string.IsNullOrWhiteSpace(request.Email))
            //{
            //    throw new BadRequestException(ResponseMessages.InvalidRequest);
            //}

            //var user = await _userManager.FindByEmailAsync(request.Email) ??
            //    throw new NotFoundException(ResponseMessages.UserNotFoundWithEmail);

            //var otpEntry = await _crudKit
            //    .AsQueryable<OtpEntry>(o => o.UserId.Equals(user.Id) && o.Type == OtpType.AccountVerification, true)
            //.OrderByDescending(o => o.ExpiresAt)
            //    .FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException(ResponseMessages.InvalidOTP);

            //bool isValid = CommonHelpers.VerifyOtp(request.Otp, otpEntry.OtpHash, otpEntry.OtpSalt)
            //              && otpEntry.ExpiresAt.IsLaterThan(DateTime.UtcNow);

            //if (!isValid)
            //{
            //    throw new ForbiddenException(ResponseMessages.OTPExpired);
            //}

            //user.EmailConfirmed = true;
            //user.UpdatedAt = DateTime.UtcNow;
            //user.Status = UserStatus.Active;
            //await _userManager.UpdateAsync(user);
            //await _crudKit.DeleteAsync(otpEntry, cancellation: cancellationToken);

            //return new ApiResult<string>("Account successfully verified. Please proceed to login");
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
    }
}