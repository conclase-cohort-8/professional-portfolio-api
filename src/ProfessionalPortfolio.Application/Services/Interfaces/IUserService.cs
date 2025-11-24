using ProfessionalPortfolio.Application.Users.Commands;
using ProfessionalPortfolio.Application.Users.Dtos;
using ProfessionalPortfolio.Application.Users.Queries;

namespace ProfessionalPortfolio.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task Delete(Guid id);
        List<UserInfoDto> GetAll(GetAllUsersQuery query);
        Task<UserInfoDto?> GetById(Guid id);
        Task<UserInfoDto> RegisterAsync(RegisterUserCommand command);
        Task<UserInfoDto?> Update(Guid id, UserUpdateCommand command);
    }
}