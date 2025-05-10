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
        public async Task<IActionResult> GetNotifications([FromQuery] GetNotificationsRequest request)
        {
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }

        [HttpPost("remove-notification")]
        public async Task<IActionResult> RemoveNotification([FromBody] RemoveNotificationRequest request)
        {
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }

        [HttpPost("read-notification")]
        public async Task<IActionResult> ReadNotification([FromBody] ReadNotificationRequest request)
        {
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }

        [HttpDelete("clear-notifications")]
        public async Task<IActionResult> ClearNotifications([FromQuery] ClearNotificationRequest request)
        {
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }
    }
}