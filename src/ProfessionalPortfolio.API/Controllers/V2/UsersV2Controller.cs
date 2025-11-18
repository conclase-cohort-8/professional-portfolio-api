using Microsoft.AspNetCore.Mvc;
using ProfessionalPortfolio.Application.Services.Interfaces;

namespace ProfessionalPortfolio.API.Controllers.V2
{
    [Route("api/v{version:apiversion}/users")]
    [ApiVersion("2.0")]
    [ApiController]
    public class UsersV2Controller : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersV2Controller(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBidAsync([FromRoute] Guid id)
        {
            var user = await _userService.GetById(id);
            return Ok(user);
        }
    }
}
