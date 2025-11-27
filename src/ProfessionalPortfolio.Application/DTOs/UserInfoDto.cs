namespace ProfessionalPortfolio.Application.DTOs
{
    public record UserInfoDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }
        public string? ResumeUrl { get; set; }
    }
}
