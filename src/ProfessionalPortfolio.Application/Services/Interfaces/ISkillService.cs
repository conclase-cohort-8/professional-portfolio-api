using ProfessionalPortfolio.Application.Commands;
using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Application.DTOs;

namespace ProfessionalPortfolio.Application.Services.Interfaces
{
    public interface ISkillService
    {
        Task<ApiResult<List<SkillInfoDto>>> AddSkills(List<string> commands);
        Task<ApiResult<string>> AddSkillsAsync(AddUserSkillCommand command);
        ApiResult<List<SkillInfoDto>> GetAllSkills();
        Task<ApiResult<List<string>>> GetUserSkillsAsync();
        Task<ApiResult<string>> RemoveUserSkill(Guid skillId);
    }
}
