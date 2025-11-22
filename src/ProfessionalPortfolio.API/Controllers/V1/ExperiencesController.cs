using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfessionalPortfolio.Application.Services.Interfaces;

namespace ProfessionalPortfolio.API.Controllers.V1
{
    [Route("api/v{version:apiversion}/experiences")]
    [ApiVersion("1.0")]
    [ApiController]
    public class ExperiencesController : ApiControllerBase
    {
        private readonly IExperienceService _service;

        public ExperiencesController(IExperienceService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var response = await _service.GetAllAsync();
            return FromResponse(response);
        }

        // TODO: Implement endpoint to add a new Experience record.
        // Paramter: AddExperienceCommand command, from body
        // Method: Post
        // Path: /
        // Returns: see above
        // Note: Call AddExperience() from the service and await it

        // TODO: Implement endpoint to update an existing Experience record.
        // Parameter: Guid id, from route
        // Paramter: UpdateExperienceCommand command, from body
        // Method: Put
        // Path: /{id}
        // Returns: see above
        // Note: Call UpdateExperience(id, command) from the service and await it
    }
}