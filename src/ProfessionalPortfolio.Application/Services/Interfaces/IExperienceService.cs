using ProfessionalPortfolio.Application.Commands;
using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Application.DTOs;

namespace ProfessionalPortfolio.Application.Services.Interfaces
{
    public interface IExperienceService
    {
        Task<ApiResult<string>> AddExperience(AddExperienceCommand command);
        Task<ApiResult<List<ExperienceInfoDto>>> GetAllAsync();
        Task<ApiResult<string>> UpdateExperience(Guid id, UpdateExperienceCommand command);
    }
}
