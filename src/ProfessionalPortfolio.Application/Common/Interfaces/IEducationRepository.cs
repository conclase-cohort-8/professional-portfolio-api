using ProfessionalPortfolio.Domain.Entities;

namespace ProfessionalPortfolio.Application.Common.Interfaces
{
    public interface IEducationRepository
    {
        Task AddAsync(Domain.Entities.Education education);
        IQueryable<Domain.Entities.Education> GetAll();
        Task<Domain.Entities.Education?> GetByIdAsync(Guid id);
    }
}