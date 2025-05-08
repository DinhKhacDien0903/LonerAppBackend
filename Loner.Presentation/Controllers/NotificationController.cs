using Loner.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Loner.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : BaseController
    {
        private readonly IMediator _mediator;
        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-notifications")]
        public async Task<IActionResult> GetMatches([FromQuery] GetNotificationsRequest request)
        {
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }
    }
}