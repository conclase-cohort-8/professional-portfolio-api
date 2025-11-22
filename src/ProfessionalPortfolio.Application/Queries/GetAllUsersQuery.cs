using ProfessionalPortfolio.Domain.Enums;

namespace ProfessionalPortfolio.Application.Queries
{
    public class GetAllUsersQuery
    {
        public string? Search { get; set; }
        public Roles? Role { get; set; }
    }
}