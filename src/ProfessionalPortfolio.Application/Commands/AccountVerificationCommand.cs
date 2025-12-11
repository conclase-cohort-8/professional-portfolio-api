using System.ComponentModel.DataAnnotations;

namespace ProfessionalPortfolio.Application.Commands
{
    public class AccountVerificationCommand
    {
        public string Email { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;
    }
}
