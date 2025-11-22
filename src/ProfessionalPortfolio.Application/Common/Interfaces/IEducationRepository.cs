using ProfessionalPortfolio.Domain.Entities;

namespace ProfessionalPortfolio.Application.Common.Interfaces
{
    public interface IEducationRepository
    {
        Task AddAsync(Education education);
        Task Deprecate(Education education);
        IQueryable<Education> GetAll();
        Task<Education?> GetByIdAsync(Guid id);
        Task UpdateAsync(Education education);
    }
}