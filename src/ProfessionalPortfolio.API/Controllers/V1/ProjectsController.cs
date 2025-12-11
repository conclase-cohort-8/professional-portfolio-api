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


        [HttpPost("AddProject")]
        [Authorize]
        public async Task<IActionResult> PostAddProject([FromBody] AddProjectCommand command)

        {
            var result = await _service.AddProject(command);
            return StatusCode(result.Status, result);
        }
    }
}
