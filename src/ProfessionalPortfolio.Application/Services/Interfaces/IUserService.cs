using ProfessionalPortfolio.Application.Commands;
using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Application.DTOs;
using ProfessionalPortfolio.Application.Queries;

namespace ProfessionalPortfolio.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task Delete(Guid id);
        List<UserInfoDto> GetAll(GetAllUsersQuery query);
        Task<UserInfoDto?> GetById(Guid id);
        Task<UserInfoDtoV2?> GetByIdV2Async(Guid id);
        Task<ApiResult<TokenDto>> LoginAsync(LoginCommand command);
        Task<UserInfoDto> RegisterAsync(RegisterUserCommand command);
        Task<UserInfoDto?> Update(UserUpdateCommand command);
    }
}
