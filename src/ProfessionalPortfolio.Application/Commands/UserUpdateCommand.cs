using System.ComponentModel.DataAnnotations;

namespace ProfessionalPortfolio.Application.Commands
{
    public record UserUpdateCommand
    {
        [Required, StringLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [Required, StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        public string? OtherName { get; set; }
    }
}