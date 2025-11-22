namespace ProfessionalPortfolio.Application.DTOs
{
    public class ExperienceInfoDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Organization { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
