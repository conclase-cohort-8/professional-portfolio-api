using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Application.Skills.Commands;
using ProfessionalPortfolio.Application.Skills.Dtos;

namespace ProfessionalPortfolio.Application.Services.Interfaces
{
    public interface ISkillService
    {
        Task<ApiResult<List<SkillInfoDto>>> AddSkills(List<string> commands);
        Task<ApiResult<string>> AddSkillsAsync(Guid userId, AddUserSkillCommand command);
        ApiResult<List<SkillInfoDto>> GetAllSkills();
        Task<ApiResult<List<string>>> GetUserSkillsAsync(Guid userId);
    }
}
