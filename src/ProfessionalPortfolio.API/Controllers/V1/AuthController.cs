using Microsoft.AspNetCore.Mvc;
using ProfessionalPortfolio.Application.Commands;
using ProfessionalPortfolio.Application.Services.Interfaces;

namespace ProfessionalPortfolio.API.Controllers.V1
{
    [Route("api/v{version:apiversion}/auth")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AuthController : ApiControllerBase
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
            return Ok(registerResult);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsyn([FromBody] LoginCommand command)
        {
            var loginResult = await _userService.LoginAsync(command);
            return FromResponse(loginResult);
        }

        [HttpPatch("verify")]
        public async Task<IActionResult> VerifyAsyn([FromBody] AccountVerificationCommand command)
        {
            var result = await _userService.VerifyAccountAsync(command);
            return FromResponse(result);
        }
    }
}