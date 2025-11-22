namespace ProfessionalPortfolio.Application.Commands
{
    public record UserUpdateCommand
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? OtherName { get; set; }
    }
}