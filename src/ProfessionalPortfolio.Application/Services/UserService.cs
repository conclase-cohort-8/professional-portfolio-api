using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProfessionalPortfolio.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<AppUser> _hasher;
        private readonly JwtOptions _jwtOptions;
        private readonly ClaimsPrincipal? _user;

        public UserService(IRepositoryManager repository, IMapper mapper, 
            IPasswordHasher<AppUser> hasher, IOptions<JwtOptions> jwt,
            IHttpContextAccessor contextAccessor)
        {
            _repository = repository;
            _mapper = mapper;
            _hasher = hasher;
            _jwtOptions = jwt.Value;
            _user = contextAccessor.HttpContext?.User;
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

            //TODO: Go to AppUser model class. Change the Status default value t Pending.
            //TODO: if the Status of the user is not Active, return error message and status:
            // Message: You can't login right now. Account is not active yet.
            // Status: 403

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
            //TODO: Got to UserUpdateCommand.cs . Add validation for First and Last name properties.
            //Both required and string length of 100.

            //TODO: Get the logged in user id below. See the AddEducation method in EducationService.cs
            var userId = Guid.Empty;
            //TODO: Return null if userId is Guid.Empty
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