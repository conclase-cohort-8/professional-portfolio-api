using System.ComponentModel.DataAnnotations;

namespace ProfessionalPortfolio.Domain.Entities
{
    public class Skill : BaseEntity
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public List<UserSkill> UserSkills { get; set; } = [];
    }
}