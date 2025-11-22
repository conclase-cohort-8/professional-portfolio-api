using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfessionalPortfolio.Application.Commands;
using ProfessionalPortfolio.Application.Queries;
using ProfessionalPortfolio.Application.Services.Interfaces;

namespace ProfessionalPortfolio.API.Controllers.V1
{
    [Route("api/v{version:apiversion}/users")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll([FromQuery] GetAllUsersQuery query)
        {
            return Ok(_userService.GetAll(query));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var response = await _userService.GetById(id);
            if(response == null)
            {
                return NotFound("User not dound");
            }
            return Ok(response);
        }

        [HttpPut]
        //TODO: Make this a protected route by adding the Authorize attirbute here
        public async Task<IActionResult> Update([FromBody] UserUpdateCommand command)
        {
            var response = await _userService.Update(command);
            if(response == null)
            {
                return NotFound("User not found");
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        //TODO: Make this a protected route by adding the Authorize attirbute here but only user with Admin role should be able to access it
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userService.Delete(id);
            return NoContent();
        }
    }
}