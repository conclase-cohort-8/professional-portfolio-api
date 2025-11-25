using Microsoft.AspNetCore.Mvc;
using ProfessionalPortfolio.Application.Commands;
using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Application.Services.Interfaces;
using ProfessionalPortfolio.Application.Users.Commands;

namespace ProfessionalPortfolio.API.Controllers.V1
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public AuthController(IUserService service,
            IEmailService emailService)
        {
            _userService = service;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMail([FromBody] EmailRequest request)
        {
            await _emailService.SendEmailAsync(request);
            return NoContent();
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
