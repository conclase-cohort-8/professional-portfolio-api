using ProfessionalPortfolio.Domain.Enums;

namespace ProfessionalPortfolio.Domain.Entities
{
    public class OtpEntry : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public string OtpHash { get; set; } = string.Empty;
        public string OtpSalt { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddMinutes(5);
        public OtpType Type { get; set; }
    }
}
