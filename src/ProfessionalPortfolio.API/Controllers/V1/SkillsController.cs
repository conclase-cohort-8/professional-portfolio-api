using Microsoft.AspNetCore.Mvc;
using ProfessionalPortfolio.Application.Services.Interfaces;
using ProfessionalPortfolio.Application.Skills.Commands;
using System.Diagnostics;

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
            if (response.Status == 200)
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
        //2. Extract the AddUserSkillCommand object from the request body.
        //3. Call the skill service method AddSkillsAsync with the UserId from step 1 and the command from step 2.
        //4. Wait for the service to return a response.
        //5. Return StatusCode() using:
        //      - The status code from the service response as the first parameter
        //      - The response body from the service as the second parameter

        [HttpPost("user")]
        public async Task<IActionResult> PostUserSkills([FromBody] AddUserSkillCommand command)
        {
            try
            {
                if (!Request.Headers.TryGetValue("X-UserId", out var userIdHeader) || string.IsNullOrEmpty(userIdHeader))
                {
                    return BadRequest("Invalid or missing X-UserId Header");
                }

                if (!Guid.TryParse(userIdHeader, out var skillIdHeader))
                {
                    return BadRequest("Invalid UserID request");
                }

                //2 
                if (command == null)
                {
                    return BadRequest("Request body cannot be empty");
                }

                //3
                var result = await _skillService.AddSkillsAsync(skillIdHeader, command);


                //5
                return StatusCode(result.StatusCode, new { Message = result.Message });


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUserSkills([FromHeader(Name = "X-UserId")] Guid userId)
        {
            var response = await _skillService.GetUserSkillsAsync(userId);
            return StatusCode(response.Status, response);
        }
    }
}

