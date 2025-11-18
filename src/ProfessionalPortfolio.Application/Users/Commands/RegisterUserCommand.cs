using ProfessionalPortfolio.Application.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ProfessionalPortfolio.Application.Users.Commands
{
    public record RegisterUserCommand
    {

        [Required, StringLength(100, MinimumLength = 2)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        public string? OtherName { get; set; }
        [Required, EmailAddress, DisposableEmailDomain]
        public string EmailAddress { get; set; } = string.Empty;
        [Required, Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
