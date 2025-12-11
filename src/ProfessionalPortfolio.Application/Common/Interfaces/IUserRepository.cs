using ProfessionalPortfolio.Domain.Entities;
using System.Linq.Expressions;

namespace ProfessionalPortfolio.Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task DeleteOtp(OtpEntry otp);
        Task<OtpEntry?> GetOtpAsync(Expression<Func<OtpEntry, bool>> predicate);
        Task InsertOtp(OtpEntry otp, bool save = true);
    }
}
