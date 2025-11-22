using ProfessionalPortfolio.Application.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ProfessionalPortfolio.Application.Commands
{
    public abstract class BaseExperienceCommand
    {
        [Required, StringLength(200)]
        public string Company { get; set; } = string.Empty;
        [Required, StringLength(200)]
        public string JobTitle { get; set; } = string.Empty;
        [Required, ValidDate]
        public DateTime StartDate { get; set; }
        [ValidDate]
        public DateTime? EndDate { get; set; }
        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
