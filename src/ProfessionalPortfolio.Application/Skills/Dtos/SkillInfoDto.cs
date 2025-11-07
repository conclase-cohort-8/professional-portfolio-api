using ProfessionalPortfolio.Domain.Entities;

namespace ProfessionalPortfolio.Application.Skills.Dtos
{
    public class SkillInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public static SkillInfoDto Map(Skill skill)
        {
            return new SkillInfoDto { Id = skill.Id, Name = skill.Name };
        }
    }
}
