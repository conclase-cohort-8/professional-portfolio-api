using ProfessionalPortfolio.Domain.Entities;

namespace ProfessionalPortfolio.Application.Common.Interfaces
{
    public interface IProjectRepository
    {
        Task AddAsync(Project project);
        Task Deprecate(Project project);
        IQueryable<Project> GetAll();
        Task<Project?> GetByIdAsync(Guid id);
        Task<List<Project>> GetByUserIdAsync(string userId);
        Task UpdateAsync(Project project);
    }
}
