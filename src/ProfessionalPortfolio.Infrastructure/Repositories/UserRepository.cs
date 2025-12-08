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