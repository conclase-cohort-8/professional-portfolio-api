using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Application.Services.Interfaces;
using ProfessionalPortfolio.Application.Users.Commands;
using ProfessionalPortfolio.Application.Users.Dtos;

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
            // You check email by calling GetByEmail() method from the repository
            // 2. If existing user not null, return null as the response
            // 3. if the user is null, Create a new AppUser object
            // 3. Save it in the in-memory repository
            // 4. Return the created user info dto: return new UserInfoDto(user);

            throw new NotImplementedException();
        }

        public List<UserInfoDto> GetAll()
        {
            var users = _repository.GetAll()
                .ToList();

            return users.Select(u => new UserInfoDto(u))
                .ToList();
        }
    }
}