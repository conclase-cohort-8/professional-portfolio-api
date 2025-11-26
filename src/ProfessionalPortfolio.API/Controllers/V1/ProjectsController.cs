using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfessionalPortfolio.Application.Commands;
using ProfessionalPortfolio.Application.Services.Interfaces;
using ProfessionalPortfolio.Domain.Entities;

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

        // TODO: POST - Implement endpoint to add a new Project record.
        [HttpPost("AddProject")]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] AddProjectCommand command)
        // Paramter: AddProjectCommand command, from body
        // Method: Post
        // Path: /
        // Returns: see above
        // Note: Call UpdateProject() from the service and await it
        {
            var response = await _service.UpdateProject();
            return FromResponse(response);
        }


        // TODO: PATCH - Implement endpoint to update an existing Project record.
        // Parameter: Guid id, from route
        // Paramter: UpdateProjectCommand command, from body
        // Method: Patch
        // Path: /{id}
        // Returns: see above
        // Note: Call UpdateProject(id, command) from the service and await it
    }
}
