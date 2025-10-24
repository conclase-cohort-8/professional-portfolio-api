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
                Id = new Guid("83bdc8b8-7cef-402c-8931-e35726c59692"),
                FirstName = "Toba",
                LastName = "Ojo",
                OtherName = "Rufus",
                Email = "ojotobar@gmail.com",
                Role = Roles.Admin.ToString()
            }
        };

        public static List<Education> Educations { get; set; } = [];
    }
}