using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Loner.Application.DTOs.Matches;
using static Loner.Application.DTOs.Profile;
using static Loner.Application.DTOs.Swipe;

namespace Loner.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //TODO: Add authorization.
    // [Authorize]
    public class SwipeController : BaseController
    {
        private readonly IMediator _mediator;

        public SwipeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Swipe([FromBody] SwipeRequest request)
        {
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }

        [HttpGet("profiles")]
        public async Task<IActionResult> GetProfiles([FromQuery] GetProfilesRequest request)
        {
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }

        [HttpGet("matches")]
        public async Task<IActionResult> GetMatches([FromQuery] GetMatchesRequest request)
        {
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }
    }
}