using Microsoft.AspNetCore.Mvc;
using ProfessionalPortfolio.Application.Services.Interfaces;
using ProfessionalPortfolio.Application.Users.Commands;

namespace ProfessionalPortfolio.API.Controllers.V1
{
    [Route("api/v{version:apiversion}/auth")]
    [ApiVersion("1.0")]
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
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request");
            }
            var registerResult = await _userService.RegisterAsync(command);
            //var json = JsonConverter.
            return Ok(registerResult);
        }
    }
}
