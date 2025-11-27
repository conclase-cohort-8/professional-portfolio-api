using ProfessionalPortfolio.Domain.Entities;

namespace ProfessionalPortfolio.Application.Common.Interfaces
{
    public interface ISkillRepository
    {
        Task AddUserSkill(UserSkill skill);
        Task CreateRangeAsync(List<Skill> skills);
        IQueryable<Skill> GetAsQueryable();
        Task<Skill?> GetByIdAsync(Guid id);
        Task<UserSkill?> GetUserSkill(string userId, Guid skillId);
        Task<List<string>> GetUserSkills(string userId);
        Task RemoveAsync(UserSkill userSkill);
    }
}
