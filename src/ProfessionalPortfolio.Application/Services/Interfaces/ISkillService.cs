using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Application.Skills.Commands;
using ProfessionalPortfolio.Application.Skills.Dtos;
using ProfessionalPortfolio.Domain.Entities;

namespace ProfessionalPortfolio.Application.Services.Interfaces
{
    public interface ISkillService
    {
        Task<ApiResult<List<SkillInfoDto>>> AddSkills(List<string> commands);
        ApiResult<List<SkillInfoDto>> GetAllSkills();
        Task<ApiResult<List<string>>> GetUserSkillsAsync(Guid userId);

        Task <ServiceResult> AddSkillsAsync (Guid skillIdHeader , AddUserSkillCommand command);
    }
}
