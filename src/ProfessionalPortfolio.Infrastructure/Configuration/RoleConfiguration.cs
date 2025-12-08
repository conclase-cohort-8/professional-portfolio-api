using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProfessionalPortfolio.Domain.Enums;

namespace ProfessionalPortfolio.Infrastructure.Configuration
{
    internal class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = new Guid("1F12A44E-9D1D-48AE-8F76-29D6DF6CAA48").ToString(),
                    Name = Roles.User.ToString(),
                    NormalizedName = Roles.User.ToString().ToUpper(),
                    ConcurrencyStamp = DateTime.UtcNow.ToString(),
                },
                new IdentityRole
                {
                    Id = new Guid("1F12A44E-9D1D-48AE-8F76-29D6DF6CAA49").ToString(),
                    Name = Roles.Admin.ToString(),
                    NormalizedName = Roles.Admin.ToString().ToUpper(),
                    ConcurrencyStamp = DateTime.UtcNow.ToString(),
                }
            );
        }
    }
}
