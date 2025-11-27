using Microsoft.EntityFrameworkCore;
using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Domain.Entities;
using ProfessionalPortfolio.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace ProfessionalPortfolio.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlServerDbContext _context;

        public UserRepository(SqlServerDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AppUser user, bool save = true)
        {
            await _context.Users.AddAsync(user);
            if(save)
            {
                await _context.SaveChangesAsync();
            }
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

        public async Task InsertOtp(OtpEntry otp, bool save = true)
        {
            await _context.Otps.AddAsync(otp);
            if(save)
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task<OtpEntry?> GetOtpAsync(Expression<Func<OtpEntry, bool>> predicate)
        {
            return await _context.Otps
                .Where(predicate)
                .OrderByDescending(o => o.Expires)
                .FirstOrDefaultAsync();
        }

        public async Task DeleteOtp(OtpEntry otp)
        {
            _context.Otps.Remove(otp);
            await _context.SaveChangesAsync();
        }
    }
}