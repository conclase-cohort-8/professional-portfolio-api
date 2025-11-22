using Microsoft.AspNetCore.Identity;
using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Domain.Entities;
using ProfessionalPortfolio.Domain.Enums;

namespace ProfessionalPortfolio.API.Extensions
{
    public static class WebAppExtensions
    {
        private const string emailAddress = "info@email.com";
        private const string key = "Pa55w0rd";

        public static async Task SeedAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            await SeedAdminUser(scope);
        }

        private static async Task SeedAdminUser(IServiceScope scope)
        {
            var repository = scope.ServiceProvider.GetRequiredService<IRepositoryManager>();
            var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<AppUser>>();

            var user = await repository.User.GetByEmailAsync(emailAddress);
            if (user == null)
            {
                user = new AppUser
                {
                    FirstName = "Admin",
                    LastName = "User",
                    Status = Statuses.Active,
                    Role = Roles.Admin.ToString(),
                    Email = emailAddress
                };

                user.PasswordHash = hasher.HashPassword(user, key);
                await repository.User.AddAsync(user);
                return;
            }
        }
    }
}