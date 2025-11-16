using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Domain.Entities;
using ProfessionalPortfolio.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfessionalPortfolio.Infrastructure.Repositories
{
    public class ExperienceRepository : IExperienceRepository
    {
        private readonly SqlServerDbContext _context;

        public ExperienceRepository(SqlServerDbContext context)
        {
            _context = context;
            
        }
        public async Task<Experience> AddAsync(Experience experience)
        {
          await _context.Experiences.AddAsync(experience);
          await _context.SaveChangesAsync();
            return experience;
        }
    }
}
