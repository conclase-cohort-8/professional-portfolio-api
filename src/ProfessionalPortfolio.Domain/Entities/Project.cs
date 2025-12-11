using System.ComponentModel.DataAnnotations;

namespace ProfessionalPortfolio.Domain.Entities
{
    public class Project : BaseEntity
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required, StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
        public AppUser? User { get; set; }
    }
}