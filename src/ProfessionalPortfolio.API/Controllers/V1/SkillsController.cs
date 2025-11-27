using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfessionalPortfolio.Application.Commands;
using ProfessionalPortfolio.Application.Services.Interfaces;

namespace ProfessionalPortfolio.API.Controllers.V1
{
    [Route("api/v{version:apiversion}/skills")]
    [ApiVersion("1.0")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillService _skillService;

        public SkillsController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostMany(List<string> skills)
        {
            var response = await _skillService.AddSkills(skills);
            if(response.Status == 200)
            {
                return Ok(response);
            }

            return StatusCode(response.Status, response);
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAll()
        {
            return Ok(_skillService.GetAllSkills());
        }

        [HttpPost("user")]
        [Authorize]
        public async Task<IActionResult> PostUserSkills([FromBody] AddUserSkillCommand command)
        {
            var result = await _skillService.AddSkillsAsync(command);
            return StatusCode(result.Status, result);
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetUserSkills([FromHeader(Name = "X-UserId")] Guid userId)
        {
            var response = await _skillService.GetUserSkillsAsync();
            return StatusCode(response.Status, response);
        }
    }
}
