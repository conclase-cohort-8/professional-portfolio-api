using System.ComponentModel.DataAnnotations;

namespace ProfessionalPortfolio.Application.Commands
{
    public class AccountVerificationCommand
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Otp { get; set; } = string.Empty;
    }
}
