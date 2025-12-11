using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Domain.Entities;
using ProfessionalPortfolio.Domain.Enums;
using System.Net;
using System.Text.Json;

namespace ProfessionalPortfolio.API.Extensions
{
    public static class WebAppExtensions
    {
        private const string emailAddress = "info@email.com";
        private const string key = "P@55w0rd";

        public static async Task SeedAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            //await SeedAdminUser(scope);
        }

        private static async Task SeedAdminUser(IServiceScope scope)
        {
            var repository = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<AppUser>>();

            var user = await repository.FindByEmailAsync(emailAddress);
            if (user == null)
            {
                user = new AppUser
                {
                    FirstName = "Admin",
                    LastName = "User",
                    Status = Statuses.Active,
                    Email = emailAddress,
                    EmailConfirmed = true,
                    UserName = emailAddress
                };

                var createResult = await repository.CreateAsync(user, key);
                if (createResult.Succeeded)
                {
                    var roleResult = await repository.AddToRoleAsync(user, Roles.Admin.ToString());
                    if (!roleResult.Succeeded)
                    {
                        await repository.DeleteAsync(user);
                    }
                }

                return;
            }
        }

        internal static void UseGlobalExceptionHandler(this WebApplication app, ILogger<Program> logger)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async ctx =>
                {
                    ctx.Response.ContentType = "application/json";
                    var ctxFeature = ctx.Features.Get<IExceptionHandlerFeature>();

                    if(ctxFeature != null)
                    {
                        logger.LogError("An error occured: {Error}", ctxFeature.Error);
                        var message = ctxFeature.Error.Message;

                        ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        await ctx.Response.WriteAsync(JsonSerializer.Serialize(new ApiResult<string>(message, ctx.Response.StatusCode)));
                    }
                });
            });
        }
    }
}