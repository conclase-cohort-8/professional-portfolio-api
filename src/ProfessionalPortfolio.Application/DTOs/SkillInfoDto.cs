using ProfessionalPortfolio.Domain.Entities;

namespace ProfessionalPortfolio.Application.DTOs
{
    public class SkillInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
