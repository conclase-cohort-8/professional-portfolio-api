using ProfessionalPortfolio.Domain.Entities;

namespace ProfessionalPortfolio.Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(AppUser user);
        Task DeleteAsync(AppUser user);
        IQueryable<AppUser> GetAll();
        Task<AppUser?> GetByEmailAsync(string email);
        Task<AppUser?> GetByIdAsync(Guid id);
        Task UpdateAsync(AppUser user);
    }
}
