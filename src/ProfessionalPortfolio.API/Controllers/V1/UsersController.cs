using Microsoft.AspNetCore.Mvc;
using ProfessionalPortfolio.Application.Services.Interfaces;

namespace ProfessionalPortfolio.API.Controllers.V1
{
    [Route("api/v1/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_userService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var response = await _userService.GetById(id);
            return Ok(response);
        }
    }
}