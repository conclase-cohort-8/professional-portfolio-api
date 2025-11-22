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
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        [Required,StringLength(20)]
        public string Role { get; set; } = Roles.Admin.ToString();
        [Required]
        public Statuses Status { get; set; } = Statuses.Active;

        // Navigation properties
        // 1 to Many relationship
        public List<Education> Educations { get; set; } = [];
        public List<Project> Projects { get; set; } = [];
        public List<Experience> Experiences { get; set; } = [];
        //1 to 1 relationship
        public Location? Location { get; set; }
        //many to many relationship
        public List<UserSkill> UserSkills { get; set; } = [];
    }
}
