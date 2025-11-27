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
    public class UsersController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetLoggedInUser()
        {
            return FromResponse(await _userService.GetLoggedInUser());
        }

        [HttpPost("upload-image")]
        [Authorize]
        public async Task<IActionResult> UploadProfilePicture(IFormFile image)
        {
            return FromResponse(await _userService.UploadProfileImage(image));
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
        [Authorize]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userService.Delete(id);
            return NoContent();
        }
    }
}