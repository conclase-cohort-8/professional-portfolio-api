namespace ProfessionalPortfolio.Application.Users.Commands
{
    public record RegisterUserCommand(string FirstName, string LastName, string? OtherName, string EmailAddress);
}
