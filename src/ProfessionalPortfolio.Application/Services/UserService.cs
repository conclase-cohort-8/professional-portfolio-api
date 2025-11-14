using AutoMapper;
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
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserInfoDto> RegisterAsync(RegisterUserCommand command)
        {
            if(string.IsNullOrWhiteSpace(command.FirstName)  || string.IsNullOrWhiteSpace(command.LastName))
            {
                return null!;
            }

            var existingUser = await _repository.GetByEmailAsync(command.EmailAddress);
            if(existingUser != null)
            {
                return null!;
            }

            var appUser = _mapper.Map<AppUser>(command);

            await _repository.AddAsync(appUser);
            return _mapper.Map<UserInfoDto>(appUser);
        }

        public async Task<UserInfoDto?> GetById(Guid id)
        {
            var user = await _repository.GetByIdAsync(id);
            if(user == null)
            {
                return null;
            }
            return _mapper.Map<UserInfoDto>(user);
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

            return _mapper.Map<List<UserInfoDto>>(users);
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

            existing.FirstName = command.FirstName;
            existing.LastName = command.LastName;
            existing.OtherName = command.OtherName;

            _mapper.Map(command, existing);
            await _repository.UpdateAsync(existing);

            return _mapper.Map<UserInfoDto>(existing);
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