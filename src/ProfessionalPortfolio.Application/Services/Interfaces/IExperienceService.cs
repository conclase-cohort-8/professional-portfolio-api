using ProfessionalPortfolio.Application.Commands;
using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Application.DTOs;

namespace ProfessionalPortfolio.Application.Services.Interfaces
{
    public interface IExperienceService
    {
        Task<ApiResult<string>> AddAsync(AddExperienceCommand command);
        Task<ApiResult<string>> DeleteAsync(Guid id);
        Task<ApiResult<List<ExperienceInfoDto>>> GetAllAsync();
        Task<ApiResult<ExperienceInfoDto>> GetByIdAsync(Guid id);
        Task<ApiResult<string>> UpdateAsync(Guid id, UpdateExperienceCommand command);
    }
}
