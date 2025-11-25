using ProfessionalPortfolio.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfessionalPortfolio.Application.Common.Interfaces
{
    public interface IExperienceRepository
    {
        Task<Experience> AddAsync(Experience experience);
    }
}
