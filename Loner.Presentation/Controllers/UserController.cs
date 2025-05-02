using Loner.Application.DTOs;
using Loner.Presentation.SwaggerDataExample;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.Security.Claims;
using static Loner.Application.DTOs.Location;
using static Loner.Application.DTOs.ProfileDetail;
using static Loner.Application.DTOs.User;

namespace Loner.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [SwaggerRequestExample(typeof(GetProfileDetailRequest), typeof(DetailProfileRequestExample))]
        [HttpGet("profile-detail")]
        public async Task<IActionResult> GetMatches([FromQuery] GetProfileDetailRequest request)
        {
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }

        [HttpPost("update-Location")]
        public async Task<IActionResult> UpdateLocationAsync([FromBody] UpdateLocationRequest request)
        {
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }

        [HttpPost("get-by-location-radius")]
        public async Task<IActionResult> GetMemberByLocationAndRadiusAsync([FromBody] GetMemberByLocationAndRadiusRequest request)
        {
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }

        [SwaggerRequestExample(typeof(UpdateUserInforRequest), typeof(UpdateUserInforRequestExample))]
        [HttpPost("update-profile")]
        public async Task<IActionResult> UpdateUserInforAsync([FromBody] UpdateUserInforRequest request)
        {
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }


        [SwaggerRequestExample(typeof(UpdateUserSettingRequest), typeof(UpdateUserSettingRequestExample))]
        [HttpPost("update-setting-account")]
        public async Task<IActionResult> UpdateUserSettingAsync([FromBody] UpdateUserSettingRequest request)
        {
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }

        [HttpPost("update-token")]
        public async Task<IActionResult> UpdateTokenAsync([FromBody] UpdateResfreshTokenRequest request)
        {
            request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }
    }
}