using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProfessionalPortfolio.Application.Services.Interfaces;
using ProfessionalPortfolio.Domain.Entities;

namespace ProfessionalPortfolio.API.Filters
{
    public class ApiKeyFilter : IAsyncActionFilter
    {
        private readonly UserManager<AppUser> _userManager;

        public ApiKeyFilter(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var headers = context.HttpContext.Request.Headers;
            if (!headers.TryGetValue("X-ApiKey", out var value) && string.IsNullOrWhiteSpace(value.ToString()))
            {
                context.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                    Content = "Permission denied"
                };

                return;
            }
            else
            {
                var user = await _userManager.FindByIdAsync(value.ToString());
                if (user == null)
                {
                    context.Result = new ContentResult
                    {
                        StatusCode = StatusCodes.Status403Forbidden,
                        Content = "Permission denied"
                    };

                    return;
                }

                await next();
            }
        }
    }
}
