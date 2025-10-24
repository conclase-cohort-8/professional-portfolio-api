using Microsoft.AspNetCore.Mvc;
using ProfessionalPortfolio.Application.Common;

namespace ProfessionalPortfolio.API.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult FromResponse<T>(ApiResult<T> response)
        {
            if (response.Success)
                return Ok(response);

            return StatusCode(response.Status, response);
        }
    }
}