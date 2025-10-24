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

        public async Task<AppUser?> GetByEmail(string email)
        {
            await Task.CompletedTask;
            return _dbContext.FirstOrDefault(u => u.Email == email);
        }

        public async Task<AppUser?> GetById(Guid id)
        {
            await Task.CompletedTask;
            return _dbContext.FirstOrDefault(u => u.Id == id);
        }

        public async Task Update(AppUser user)
        {
            await Task.CompletedTask;
            _dbContext.Add(user);
        }

        public async Task Delete(AppUser user)
        {
            await Task.CompletedTask;
            _dbContext.Remove(user);
        }
    }
}
