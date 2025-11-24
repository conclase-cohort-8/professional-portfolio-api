using Microsoft.EntityFrameworkCore;
using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Domain.Entities;
using ProfessionalPortfolio.Infrastructure.Persistence;

namespace ProfessionalPortfolio.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlServerDbContext _context;

        public UserRepository(SqlServerDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AppUser user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(AppUser user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public IQueryable<AppUser> GetAll()
        {
            return _context.Users;
        }

        public async Task<AppUser?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<AppUser?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task UpdateAsync(AppUser user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}