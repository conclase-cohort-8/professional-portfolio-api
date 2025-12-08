using ProfessionalPortfolio.Domain.Entities;

namespace ProfessionalPortfolio.Application.Common.Interfaces
{
    public interface IExperienceRepository
    {
        Task AddAsync(Experience experience);
        Task Deprecate(Experience experience);
        IQueryable<Experience> GetAll();
        Task<Experience?> GetByIdAsync(Guid id);
        Task<List<Experience>> GetByUserIdAsync(string userId);
        Task UpdateAsync(Experience experience);
    }
}
