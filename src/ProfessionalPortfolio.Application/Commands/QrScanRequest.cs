using System.ComponentModel.DataAnnotations;

namespace ProfessionalPortfolio.Application.Commands
{
    public class QrScanRequest
    {
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
