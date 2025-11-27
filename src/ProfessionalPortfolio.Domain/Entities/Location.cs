using System.ComponentModel.DataAnnotations;

namespace ProfessionalPortfolio.Domain.Entities
{
    public class Location : BaseEntity
    {
        [Required, StringLength(500)]
        public string Address { get; set; } = string.Empty;
        [Required, StringLength(100)]
        public string City { get; set; } = string.Empty;
        [Required, StringLength(200)]
        public string State { get; set; } = string.Empty;
        [Required, StringLength(200)]
        public string Country { get; set; } = string.Empty;

        // Nav Prop
        public string UserId { get; set; } = string.Empty;
        public AppUser? User { get; set; }
    }
}