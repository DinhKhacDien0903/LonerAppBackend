using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Loner.Application.DTOs.Message;

namespace Loner.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //TODO: Add authorization.
    // [Authorize]
    public class MessageController : BaseController
    {
        private readonly IMediator _mediator;
        public MessageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-user-matched-active")]
        public async Task<IActionResult> GetMatchedActiveUserAsync([FromQuery] GetMatchedActiveUserRequest request)
        {
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }

        [HttpGet("get-user-message")]
        public async Task<IActionResult> GetUserMessageAsync([FromQuery] GetBasicUserMessageRequest request)
        {
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }
    }
}