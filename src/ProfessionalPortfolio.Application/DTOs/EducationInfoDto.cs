namespace ProfessionalPortfolio.Application.DTOs
{
    public class EducationInfoDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Institution { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
    }
}