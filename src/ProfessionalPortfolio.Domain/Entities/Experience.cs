using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfessionalPortfolio.Domain.Entities
{
    public class Experience
    {
        [Required]
        public string Organization { get; set; } = string.Empty;

        [Required]
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public string Description { get; set; } = string.Empty;

        public Guid UserId { get; set; }

        public AppUser? User { get; set; }
    }
}
