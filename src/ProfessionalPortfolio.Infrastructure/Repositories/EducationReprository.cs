using Microsoft.EntityFrameworkCore;
using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Domain.Entities;
using ProfessionalPortfolio.Infrastructure.Persistence;

namespace ProfessionalPortfolio.Infrastructure.Repositories
{
    public class EducationRepository : IEducationRepository
    {
        private readonly SqlServerDbContext _dbContext;

        public EducationRepository(SqlServerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Education education)
        {
            await _dbContext.Educations.AddAsync(education);
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<Education> GetAll()
            => _dbContext.Educations;

        public async Task<Education?> GetByIdAsync(Guid id)
            => await _dbContext.Educations
            .FirstOrDefaultAsync(ed => ed.Id == id);
    }
}