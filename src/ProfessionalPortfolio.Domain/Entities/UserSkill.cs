namespace ProfessionalPortfolio.Domain.Entities
{
    public class UserSkill
    {
        public string UserId { get; set; } = string.Empty;
        public AppUser? User { get; set; }

        public Guid SkillId { get; set; }
        public Skill? Skill { get; set; }
    }
}