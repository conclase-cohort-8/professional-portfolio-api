using Microsoft.AspNetCore.Http;
using ProfessionalPortfolio.Application.Commands;
using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Application.DTOs;
using ProfessionalPortfolio.Application.Queries;

namespace ProfessionalPortfolio.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResult<string>> Delete(string id);
        List<UserInfoDto> GetAll(GetAllUsersQuery query);
        Task<UserInfoDto?> GetById(string id);
        Task<UserInfoDtoV2?> GetByIdV2Async(string id);
        Task<ApiResult<UserInfoDto>> GetLoggedInUser();
        Task<ApiResult<PagedResult<UserInfoDto>>> GetPagedUser(GetAllUsersQuery query);
        Task<ApiResult<TokenDto>> LoginAsync(LoginCommand command);
        Task<ApiResult<string>> RegisterAsync(RegisterUserCommand command);
        Task<ApiResult<string>> Update(UserUpdateCommand command);
        Task<ApiResult<string>> UploadProfileImage(IFormFile file);
        Task<ApiResult<string>> UploadUserCv(IFormFile cv);
        Task<ApiResult<string>> VerifyAccountAsync(AccountVerificationCommand command);
    }
}
