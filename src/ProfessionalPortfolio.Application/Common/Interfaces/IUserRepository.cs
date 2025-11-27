using ProfessionalPortfolio.Domain.Entities;
using System.Linq.Expressions;

namespace ProfessionalPortfolio.Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(AppUser user, bool save = true);
        Task DeleteAsync(AppUser user);
        Task DeleteOtp(OtpEntry otp);
        IQueryable<AppUser> GetAll();
        Task<AppUser?> GetByEmailAsync(string email);
        Task<AppUser?> GetByIdAsync(Guid id);
        Task<OtpEntry?> GetOtpAsync(Expression<Func<OtpEntry, bool>> predicate);
        Task InsertOtp(OtpEntry otp, bool save = true);
        Task UpdateAsync(AppUser user);
    }
}
