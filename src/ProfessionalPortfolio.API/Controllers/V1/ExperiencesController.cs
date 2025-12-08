using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfessionalPortfolio.Application.Commands;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            return FromResponse(await  _service.GetByIdAsync(id));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostAsync([FromBody] AddExperienceCommand command)
        {
            return FromResponse(await _service.AddAsync(command));
        }

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> PostAsync([FromRoute] Guid id, [FromBody] UpdateExperienceCommand command)
        {
            return FromResponse(await _service.UpdateAsync(id, command));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            return FromResponse(await _service.DeleteAsync(id));
        }
    }
}