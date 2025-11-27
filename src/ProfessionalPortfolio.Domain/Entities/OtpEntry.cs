using ProfessionalPortfolio.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProfessionalPortfolio.Domain.Entities
{
    public class OtpEntry : BaseEntity
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string OtpHash { get; set; } = string.Empty;
        [Required]
        public string OtpSalt { get; set; } = string.Empty;
        public DateTime Expires { get; set; } = DateTime.UtcNow.AddMinutes(5);
        [Required]
        public OtpType Type { get; set; }
    }
}
