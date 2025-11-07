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

        // Navigation properties
        // 1 to Many relationship
        public List<Education> Educations { get; set; } = [];
        //1 to 1 relationship
        public Location? Location { get; set; }

        public List<UserSkill> UserSkills { get; set; } = [];
    }
}
