using ProfessionalPortfolio.Domain.Entities;

namespace ProfessionalPortfolio.Application.Users.Dtos
{
    public record UserInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
