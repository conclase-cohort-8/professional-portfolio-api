
using System.ComponentModel.DataAnnotations;

namespace ProfessionalPortfolio.Application.Users.Commands
{
    public record RegisterUserCommand([Required] string FirstName, [Required] string LastName, string? OtherName, [Required] string EmailAddress);
}
