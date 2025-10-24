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
        //TODO: POST Action
        //TODO: SHOULD return Task<IActionResult>
        //TODO: Retrieve user id from the header. Header name should be X-UserId
        //TODO: Command comes from the request body. Use: AddEducationCommand type
        //TODO: CALL _service.AddEducation(userId, command)
        //TODO: RETURN FromResponse() and PASS the response from the service into it.
        #endregion
    }
}