using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProfessionalPortfolio.API.Controllers.V1
{
    [Route("api/v1/educations")]
    [ApiController]
    public class EducationsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
