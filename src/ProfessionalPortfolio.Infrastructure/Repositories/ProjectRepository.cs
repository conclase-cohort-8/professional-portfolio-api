using Microsoft.EntityFrameworkCore;
using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Domain.Entities;
using ProfessionalPortfolio.Infrastructure.Persistence;

namespace ProfessionalPortfolio.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly SqlServerDbContext _dbContext;

        public ProjectRepository(SqlServerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Project project)
        {
            await _dbContext.Projects.AddAsync(project);
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<Project> GetAll()
            => _dbContext.Projects.Where(p => !p.IsDeleted);

        public async Task<Project?> GetByIdAsync(Guid id)
            => await _dbContext.Projects
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

        public async Task<List<Project>> GetByUserIdAsync(Guid userId)
            => await _dbContext.Projects
            .Where(p => p.UserId == userId && !p.IsDeleted)
            .ToListAsync();

        public async Task UpdateAsync(Project project)
        {
            _dbContext.Projects.Update(project);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Deprecate(Project project)
        {
            project.IsDeleted = true;
            _dbContext.Projects.Update(project);
            await _dbContext.SaveChangesAsync();
        }
    }
}