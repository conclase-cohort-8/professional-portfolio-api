using ProfessionalPortfolio.Domain.Entities;

namespace ProfessionalPortfolio.Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(AppUser user);
        Task Delete(AppUser user);
        IQueryable<AppUser> GetAll();
        Task<AppUser?> GetByEmail(string email);
        Task<AppUser?> GetById(Guid id);
        Task Update(AppUser user);
    }
}
