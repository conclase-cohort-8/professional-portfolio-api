using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfessionalPortfolio.Application.Commands;
using ProfessionalPortfolio.Application.Services.Interfaces;

namespace ProfessionalPortfolio.API.Controllers.V1
{
    [Route("api/v{version:apiversion}/educations")]
    [ApiVersion("1.0")]
    [ApiController]
    public class EducationsController : ApiControllerBase
    {
        private readonly IEducationService _service;

        public EducationsController(IEducationService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var response = _service.GetEducations();
            return FromResponse(response);
        }

        #region Your Get All Action Should Go Here
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] AddEducationCommand command)
        {
            var response = await _service.AddEducation(command);
            return FromResponse(response);
        }        
        #endregion
    }
}