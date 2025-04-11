using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Loner.Application.DTOs.Auth;

namespace Loner.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] RegisterPhoneNumberRequest phoneRequest)
        {
            var result = await _mediator.Send(phoneRequest);
            return HandlResult(result);
        }

        [HttpPost("verify-otp-and-register")]
        public async Task<IActionResult> VerifyOtpAndRegister([FromBody] VerifyPhoneNumberRequest verityRequest)
        {
            var result = await _mediator.Send(verityRequest);
            return HandlResult(result);
        }
    }
}