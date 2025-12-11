using System.ComponentModel.DataAnnotations;

namespace ProfessionalPortfolio.Domain.Entities
{
    public class Experience : BaseEntity
    {
        [Required, StringLength(200)]
        public string Organization { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        public string Description { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
        public AppUser? User { get; set; }
    }
}