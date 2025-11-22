using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfessionalPortfolio.Application.Services.Interfaces;

namespace ProfessionalPortfolio.API.Controllers.V1
{
    [Route("api/v{version:apiversion}/projects")]
    [ApiVersion("1.0")]
    [ApiController]
    public class ProjectsController : ApiControllerBase
    {
        private readonly IProjectService _service;

        public ProjectsController(IProjectService service)
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

        // TODO: Implement endpoint to add a new Project record.
        // Paramter: AddProjectCommand command, from body
        // Method: Post
        // Path: /
        // Returns: see above
        // Note: Call UpdateProject() from the service and await it

        // TODO: Implement endpoint to update an existing Project record.
        // Parameter: Guid id, from route
        // Paramter: UpdateProjectCommand command, from body
        // Method: Put
        // Path: /{id}
        // Returns: see above
        // Note: Call UpdateProject(id, command) from the service and await it
    }
}
