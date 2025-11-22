using System.ComponentModel.DataAnnotations;

namespace ProfessionalPortfolio.Application.Commands
{
    public class BaseProjectCommand
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required, StringLength(2000)]
        public string Description { get; set; } = string.Empty;
    }
}
