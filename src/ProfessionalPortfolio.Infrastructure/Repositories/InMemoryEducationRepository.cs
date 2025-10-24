using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Domain.Entities;
using ProfessionalPortfolio.Infrastructure.Persistence;

namespace ProfessionalPortfolio.Infrastructure.Repositories
{
    public class InMemoryEducationRepository : IEducationRepository
    {
        private List<Education> _dbContext;

        public InMemoryEducationRepository()
        {
            _dbContext = InMemoryDbContext.Educations;
        }

        public async Task<Education?> GetByIdAsync(Guid id)
        {
            await Task.CompletedTask;
            return _dbContext.FirstOrDefault(u => u.Id == id);
        }

        public async Task AddAsync(Education education)
        {
            _dbContext.Add(education);
            await Task.CompletedTask;
        }

        public IQueryable<Education> GetAll()
        {
            return _dbContext.AsQueryable();
        }
    }
}