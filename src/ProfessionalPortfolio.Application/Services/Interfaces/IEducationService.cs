using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Application.Educations.Commands;
using ProfessionalPortfolio.Application.Educations.Dtos;

namespace ProfessionalPortfolio.Application.Services.Interfaces
{
    public interface IEducationService
    {
        Task<ApiResult<EducationInfoDto>> AddEducation(Guid userId, AddEducationCommand command);
        ApiResult<List<EducationInfoDto>> GetEducations(Guid userId);
    }
}