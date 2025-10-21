using Microsoft.AspNetCore.Mvc;
using ProfessionalPortfolio.Application.Services.Interfaces;
using ProfessionalPortfolio.Application.Users.Commands;

namespace ProfessionalPortfolio.API.Controllers.V1
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService service)
        {
            _userService = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var registerResult = await _userService.RegisterAsync(command);
            return Ok(registerResult);
        }
    }
}
