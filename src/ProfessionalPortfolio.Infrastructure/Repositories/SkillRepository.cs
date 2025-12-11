using Microsoft.EntityFrameworkCore;
using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Domain.Entities;
using ProfessionalPortfolio.Infrastructure.Persistence;

namespace ProfessionalPortfolio.Infrastructure.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly SqlServerDbContext _context;

        public SkillRepository(SqlServerDbContext context)
        {
            _context = context;
        }

        public async Task CreateRangeAsync(List<Skill> skills)
        {
            await _context.AddRangeAsync(skills);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Skill> GetAsQueryable()
            => _context.Skills;

        public async Task<Skill?> GetByIdAsync(Guid id)
            => await _context.Skills.FirstOrDefaultAsync(s => s.Id == id);

        public async Task AddUserSkill(UserSkill skill)
        {
            await _context.UserSkills.AddAsync(skill);
            await _context.SaveChangesAsync();
        }

        public async Task<List<string>> GetUserSkills(string userId)
        {
            return await _context.UserSkills
                .Where(us => us.UserId == userId)
                .Select(us => us.Skill!.Name)
                .ToListAsync();
        }

        public async Task<UserSkill?> GetUserSkill(string userId, Guid skillId)
        {
            return await _context.UserSkills
                .Where(us => us.UserId == userId && us.SkillId == skillId)
                .FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(UserSkill userSkill)
        {
            _context.UserSkills.Remove(userSkill);
            await _context.SaveChangesAsync();
        }
    }
}