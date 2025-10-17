using ProfessionalPortfolio.Domain.Entities;
using ProfessionalPortfolio.Domain.Enums;

namespace ProfessionalPortfolio.Infrastructure.Persistence
{
    public class InMemoryDbContext
    {
        public static List<AppUser> Users { get; set; } = new List<AppUser>
        {
            new AppUser
            {
                FirstName = "Toba",
                LastName = "Ojo",
                OtherName = "Rufus",
                Email = "ojotobar@gmail.com",
                Role = Roles.Admin.ToString()
            }
        };
    }
}