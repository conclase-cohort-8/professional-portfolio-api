using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProfessionalPortfolio.Application.Services.Interfaces;

namespace ProfessionalPortfolio.API.Filters
{
    public class ApiKeyFilter : IAsyncActionFilter
    {
        private readonly IUserService _userService;

        public ApiKeyFilter(IUserService userService)
        {
            _userService = userService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var headers = context.HttpContext.Request.Headers;
            if (!headers.TryGetValue("X-ApiKey", out var value) || !Guid.TryParse(value, out var userId))
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
                var user = await _userService.GetById(userId);
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