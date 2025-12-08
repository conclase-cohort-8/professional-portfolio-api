using ProfessionalPortfolio.Domain.Entities;

namespace ProfessionalPortfolio.Application.Common
{
    internal static class FilterExtensions
    {
        public static IQueryable<AppUser> Filter(this IQueryable<AppUser> users, string? search)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                users = users.Where(u => u.FirstName.ToLower().Contains(search.ToLower()) || 
                                         u.LastName.ToLower().Contains(search.ToLower()));
            }

            return users;
        }

        public static IQueryable<AppUser> Sort(this IQueryable<AppUser> users, string sortBy, bool asc = true)
        {
            return sortBy switch
            {
                "firstName" => asc ? users.OrderBy(u => u.FirstName) : users.OrderByDescending(u => u.FirstName),
                "lastName" => asc ? users.OrderBy(u => u.LastName) : users.OrderByDescending(u => u.LastName),
                _ => users
            };
        }
    }
}
