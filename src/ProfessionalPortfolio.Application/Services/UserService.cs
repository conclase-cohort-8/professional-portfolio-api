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
            if (string.IsNullOrWhiteSpace(command.FirstName) || string.IsNullOrWhiteSpace(command.LastName))
            {
                return null!;
            }

            // check for existing user
            // You check email by calling GetByEmail() method from the repository
            // 2. If existing user not null, return null as the response
            var existingUser = await _repository.GetByEmailAsync(command.EmailAddress);
            if (existingUser != null)
            {
                return null!;
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
            if (user == null)
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
            if (command == null || string.IsNullOrEmpty(command.FirstName) || string.IsNullOrEmpty(command.LastName))
            {
                return null;
            }

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
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

            await _repository.DeleteAsync(user);
        }
    }
}
