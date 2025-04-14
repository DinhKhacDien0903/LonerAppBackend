using Loner.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace Loner.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");

            return result.IsSuccess switch
            {
                true => Ok(result.Data),
                false => result.ErrorMessage switch
                {
                    //TODO: handle more specific error messages
                    "Not found" => NotFound(new { Error = result.ErrorMessage }),
                    _ => BadRequest(new { Error = result.ErrorMessage })
                }
            };
        }
    }
}