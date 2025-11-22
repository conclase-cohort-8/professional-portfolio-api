using System.ComponentModel.DataAnnotations;

namespace ProfessionalPortfolio.Application.DTOs
{
    public class ProjectInfoDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
