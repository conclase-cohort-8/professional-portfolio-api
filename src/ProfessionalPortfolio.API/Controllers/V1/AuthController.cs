using Microsoft.AspNetCore.Mvc;
using ProfessionalPortfolio.Application.Commands;
using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Application.DTOs;
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

        /// <summary>
        /// Registers new users
        /// </summary>
        /// <param name="command"></param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <response code="409">Conflict or existing user</response>
        /// <response code="500">Unexpected exception</response>
        /// <returns></returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResult<string>), 200)]
        [ProducesResponseType(typeof(ApiResult<string>), 400)]
        [ProducesResponseType(typeof(ApiResult<string>), 409)]
        [ProducesResponseType(typeof(ApiResult<string>), 500)]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var registerResult = await _userService.RegisterAsync(command);
            return FromResponse(registerResult);
        }

        /// <summary>
        /// Authenticates users
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResult<TokenDto>), 200)]
        [ProducesResponseType(typeof(ApiResult<string>), 400)]
        [ProducesResponseType(typeof(ApiResult<string>), 403)]
        [ProducesResponseType(typeof(ApiResult<string>), 404)]
        [ProducesResponseType(typeof(ApiResult<string>), 500)]
        public async Task<IActionResult> LoginAsyn([FromBody] LoginCommand command)
        {
            var loginResult = await _userService.LoginAsync(command);
            return FromResponse(loginResult);
        }

        /// <summary>
        /// Verifies user account
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPatch("verify")]
        [ProducesResponseType(typeof(ApiResult<string>), 200)]
        [ProducesResponseType(typeof(ApiResult<string>), 400)]
        [ProducesResponseType(typeof(ApiResult<string>), 404)]
        [ProducesResponseType(typeof(ApiResult<string>), 500)]
        public async Task<IActionResult> VerifyAsyn([FromBody] AccountVerificationCommand command)
        {
            var result = await _userService.VerifyAccountAsync(command);
            return FromResponse(result);
        }
    }
}