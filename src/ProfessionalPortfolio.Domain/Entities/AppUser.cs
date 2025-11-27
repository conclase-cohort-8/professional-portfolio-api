using Microsoft.AspNetCore.Identity;
using ProfessionalPortfolio.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProfessionalPortfolio.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        public string? OtherName { get; set; }
        [Required]
        public Statuses Status { get; set; } = Statuses.Pending;

        public string? ProfilePicture { get; set; }
        public string? ProfilePicturePublicId { get; set; }

        public string? ResumeUrl { get; set; }
        public string? ResumePublicId { get; set; }

        // Navigation properties
        // 1 to Many relationship
        public List<Education> Educations { get; set; } = [];
        public List<Project> Projects { get; set; } = [];
        public List<Experience> Experiences { get; set; } = [];
        //1 to 1 relationship
        public Location? Location { get; set; }
        //many to many relationship
        public List<UserSkill> UserSkills { get; set; } = [];

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
