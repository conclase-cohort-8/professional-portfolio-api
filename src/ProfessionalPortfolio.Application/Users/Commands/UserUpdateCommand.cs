namespace ProfessionalPortfolio.Application.Users.Commands
{
    public record UserUpdateCommand(string FirstName, string LastName, string? OtherName);
}