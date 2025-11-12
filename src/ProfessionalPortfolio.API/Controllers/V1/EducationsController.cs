using Microsoft.AspNetCore.Mvc;
using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Application.Educations.Commands;
using ProfessionalPortfolio.Application.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProfessionalPortfolio.API.Controllers.V1
{
    [Route("api/v1/educations")]
    [ApiController]
    public class EducationsController : ApiControllerBase
    {
        private readonly IEducationService _service;

        public EducationsController(IEducationService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get([FromHeader(Name = "X-UserId")] Guid userId)
        {

            return FromResponse(_service.GetEducations(userId));
        }

        #region Your Get All Action Should Go Here
        [HttpPost]
        public async Task<IActionResult> Post([FromHeader(Name = "X-UserId")] Guid userId, 
                                              [FromBody] AddEducationCommand command)
        {
            var response = await _service.AddEducation(userId, command);
            return FromResponse(response);
        }        
        #endregion
    }
}