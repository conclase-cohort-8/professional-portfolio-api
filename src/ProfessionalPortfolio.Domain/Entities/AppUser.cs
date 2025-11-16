using ProfessionalPortfolio.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProfessionalPortfolio.Domain.Entities
{
    public class AppUser : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        public string? OtherName { get; set; }
        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required,StringLength(20)]
        public string Role { get; set; } = Roles.User.ToString();
        [Required, StringLength(20)]
        public string Status { get; set; } = Statuses.Active.ToString();

        // Navigation properties
        // 1 to Many relationship
        public List<Education> Educations { get; set; } = [];
        //1 to 1 relationship
        public Location? Location { get; set; }

        public List<UserSkill> UserSkills { get; set; } = [];

        //updated
        public virtual ICollection<Experience> Experiences { get; set; } = [];
    }
}
