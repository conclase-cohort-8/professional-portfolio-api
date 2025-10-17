using ProfessionalPortfolio.Domain.Enums;

namespace ProfessionalPortfolio.Domain.Entities
{
    public class AppUser : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? OtherName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = Roles.User.ToString();
        public string Status { get; set; } = Statuses.Active.ToString();
    }
}
