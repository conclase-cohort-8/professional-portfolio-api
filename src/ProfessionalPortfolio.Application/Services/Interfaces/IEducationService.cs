using ProfessionalPortfolio.Application.Commands;
using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Application.DTOs;

namespace ProfessionalPortfolio.Application.Services.Interfaces
{
    public interface IEducationService
    {
        Task<ApiResult<EducationInfoDto>> AddEducation(AddEducationCommand command);
        Task<ApiResult<string>> DeleteEducation(Guid id);
        ApiResult<List<EducationInfoDto>> GetEducations();
        Task<ApiResult<string>> UpdateEducation(Guid id, UpdateEducationCommand command);
    }
}