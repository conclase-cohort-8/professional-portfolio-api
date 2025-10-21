using ProfessionalPortfolio.Application.Users.Commands;
using ProfessionalPortfolio.Application.Users.Dtos;

namespace ProfessionalPortfolio.Application.Services.Interfaces
{
    public interface IUserService
    {
        List<UserInfoDto> GetAll();
        Task<UserInfoDto?> GetById(Guid id);
        Task<UserInfoDto> RegisterAsync(RegisterUserCommand command);
    }
}
