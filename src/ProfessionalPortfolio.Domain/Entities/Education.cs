namespace ProfessionalPortfolio.Domain.Entities
{
    public class Education : BaseEntity
    {
        public string Institution { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public string FieldOfStudy { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Navigation Properties
        public Guid UserId { get; set; }
        public AppUser? User { get; set; }
    }
}