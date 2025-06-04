using Loner.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Loner.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetupController : BaseController
    {
        private readonly IMediator _mediator;
        public SetupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("setup-name")]
        public async Task<IActionResult> SetupNameAsync([FromBody] SetUpNameRequest request)
        {
            request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }

        [HttpPost("setup-dateOfBirth")]
        public async Task<IActionResult> SetupDateOfBirthAsync([FromBody] SetUpDOBRequest request)
        {
            request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }

        [HttpPost("setup-gender")]
        public async Task<IActionResult> SetupGenderBirthAsync([FromBody] SetUpGenderRequest request)
        {
            request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }

        [HttpPost("setup-gender-show-me")]
        public async Task<IActionResult> SetupGenderShowMeAsync([FromBody] SetUpShowGenderRequest request)
        {
            request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }

        [HttpPost("setup-university")]
        public async Task<IActionResult> SetupUniversityAsync([FromBody] SetUpUniversityRequest request)
        {
            request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }

        [HttpPost("setup-interests")]
        public async Task<IActionResult> SetupInterestsAsync([FromBody] SetUpInterestRequest request)
        {
            request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }

        [HttpPost("setup-photos")]
        public async Task<IActionResult> SetupPhotosAsync([FromBody] SetUpPhotosRequest request)
        {
            request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }
    }
}