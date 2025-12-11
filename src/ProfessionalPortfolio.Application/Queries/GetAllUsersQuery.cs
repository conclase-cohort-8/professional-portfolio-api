using ProfessionalPortfolio.Domain.Enums;

namespace ProfessionalPortfolio.Application.Queries
{
    public class GetAllUsersQuery
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
        public string SortBy { get; set; } = string.Empty;
        public bool IsAscending { get; set; }
    }
}