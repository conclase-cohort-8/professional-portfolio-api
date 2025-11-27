using Microsoft.EntityFrameworkCore;
using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Domain.Entities;
using ProfessionalPortfolio.Infrastructure.Persistence;

namespace ProfessionalPortfolio.Infrastructure.Repositories
{
    public class ExperienceRepository : IExperienceRepository
    {
        private readonly SqlServerDbContext _dbContext;

        public ExperienceRepository(SqlServerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Experience experience)
        {
            await _dbContext.Experiences.AddAsync(experience);
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<Experience> GetAll()
            => _dbContext.Experiences.Where(e => !e.IsDeleted);

        public async Task<List<Experience>> GetByUserIdAsync(string userId)
            => await _dbContext.Experiences
            .Where(ex => ex.UserId == userId && !ex.IsDeleted)
            .ToListAsync();

        public async Task<Experience?> GetByIdAsync(Guid id)
            => await _dbContext.Experiences
            .FirstOrDefaultAsync(ex => ex.Id == id && !ex.IsDeleted);

        public async Task UpdateAsync(Experience experience)
        {
            _dbContext.Experiences.Update(experience);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Deprecate(Experience experience)
        {
            experience.IsDeleted = true;
            experience.UpdatedOn = DateTime.UtcNow;
            _dbContext.Experiences.Update(experience);
            await _dbContext.SaveChangesAsync();
        }
    }
}
