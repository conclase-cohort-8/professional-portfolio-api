using ProfessionalPortfolio.Application.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ProfessionalPortfolio.Application.Commands
{
    public abstract record BaseEducationCommand
    {
        [Required, StringLength(200)]
        public string Institution { get; set; } = string.Empty;
        [Required, StringLength(200)]
        public string Degree { get; set; } = string.Empty;
        [Required, StringLength(200)]
        public string Course { get; set; } = string.Empty;
        [Required, ValidDate]
        public DateTime StartDate { get; set; }
        [ValidDate]
        public DateTime? EndDate { get; set; }
    }
}
