using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfessionalPortfolio.Domain.Entities
{
    public class Experience : BaseEntity
    {
        [Required]
        public string Organization { get; set; }

        [Required]
        public string JobTitle { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public string Description { get; set; }

        public Guid AppUserId { get; set; }

        public virtual AppUser AppUser { get; set; }
    }
}
