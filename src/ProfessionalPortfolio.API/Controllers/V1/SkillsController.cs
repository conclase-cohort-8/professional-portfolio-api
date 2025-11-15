using Microsoft.AspNetCore.Mvc;
using ProfessionalPortfolio.Application.Services.Interfaces;
using ProfessionalPortfolio.Application.Skills.Commands;

namespace ProfessionalPortfolio.API.Controllers.V1
{
    [Route("api/v1/skills")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillService _skillService;

        public SkillsController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        [HttpPost]
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
        public IActionResult GetAll()
        {
            return Ok(_skillService.GetAllSkills());
        }

        //TODO: Action Method: PostUserSkills
        //When a POST request is made to the "api/v1/skills/user" endpoint:
        //1. Extract UserId from the request header("X-UserId").
          [HttpPost("user")]
          public async Task<IActionResult> PostUserSkills([FromHeader(Name = "X-UserId")]Guid UserId, [FromBody]AddUserSkillCommand skillCommand)
        {
            var response = await _skillService.AddSkillsAsync(UserId, skillCommand);
                return StatusCode(response.Status, response);
        }
        //2. Extract the AddUserSkillCommand object from the request body.
        //3. Call the skill service method AddSkillsAsync with the UserId from step 1 and the command from step 2.
        //4. Wait for the service to return a response.
        //5. Return StatusCode() using:
        //      - The status code from the service response as the first parameter
        //      - The response body from the service as the second parameter



        [HttpGet("user")]
        public async Task<IActionResult> GetUserSkills([FromHeader(Name = "X-UserId")] Guid userId)
        {
            var response = await _skillService.GetUserSkillsAsync(userId);
            return StatusCode(response.Status, response);
        }
    }
}
