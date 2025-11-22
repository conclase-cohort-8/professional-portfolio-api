using ProfessionalPortfolio.Application.Commands;
using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Application.DTOs;

namespace ProfessionalPortfolio.Application.Services.Interfaces
{
    public interface IEducationService
    {
        Task<ApiResult<EducationInfoDto>> AddEducation(AddEducationCommand command);
        Task<EducationInfoDto> UpdateEducation(UpdateEducationCommand command);
        ApiResult<List<EducationInfoDto>> GetEducations();
    }
}