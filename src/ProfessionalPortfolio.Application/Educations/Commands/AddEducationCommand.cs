using System.ComponentModel.DataAnnotations;

namespace ProfessionalPortfolio.Application.Educations.Commands
{
    public record AddEducationCommand
    {
        [Required]
        public string Institution { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}