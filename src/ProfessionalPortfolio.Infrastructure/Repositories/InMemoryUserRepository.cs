using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Domain.Entities;
using ProfessionalPortfolio.Infrastructure.Persistence;

namespace ProfessionalPortfolio.Infrastructure.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private List<AppUser> _dbContext;

        public InMemoryUserRepository() 
        {
            _dbContext = InMemoryDbContext.Users ?? [];
        }

        public async Task AddAsync(AppUser user)
        {
            _dbContext.Add(user);
            await Task.CompletedTask;
        }

        public IQueryable<AppUser> GetAll()
        {
            return _dbContext.AsQueryable();
        }

        public async Task<AppUser?> GetByEmailAsync(string email)
        {
            await Task.CompletedTask;
            return _dbContext.FirstOrDefault(u => u.Email == email);
        }

        public async Task<AppUser?> GetByIdAsync(Guid id)
        {
            await Task.CompletedTask;
            return _dbContext.FirstOrDefault(u => u.Id == id);
        }

        public async Task UpdateAsync(AppUser user)
        {
            await Task.CompletedTask;
            _dbContext.Add(user);
        }

        public async Task DeleteAsync(AppUser user)
        {
            await Task.CompletedTask;
            _dbContext.Remove(user);
        }
    }
}
