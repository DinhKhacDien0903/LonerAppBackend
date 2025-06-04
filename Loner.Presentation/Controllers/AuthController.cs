using Loner.Presentation.SwaggerDataExample;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
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
            return HandleResult(result);
        }

        [HttpPost("verify-otp-and-register")]
        public async Task<IActionResult> VerifyOtpAndRegister([FromBody] VerifyPhoneNumberRequest verityRequest)
        {
            var result = await _mediator.Send(verityRequest);
            return HandleResult(result);
        }

        [HttpPost("send-mail-otp")]
        public async Task<IActionResult> SendMailOtpAsync([FromBody] RegisterEmailRequest email)
        {
            var result = await _mediator.Send(email);
            return HandleResult(result);
        }

        [HttpPost("send-mail-otp-admin")]
        public async Task<IActionResult> SendMailAdminOtpAsync([FromBody] RegisterAdminEmailRequest email)
        {
            var result = await _mediator.Send(email);
            return HandleResult(result);
        }

        [HttpPost("verify-mail-otp-and-register")]
        [SwaggerRequestExample(typeof(VerifyEmailRequest), typeof(VerifyEmailRequestExample))]
        public async Task<IActionResult> VerifyMailOtpAndRegisterAsync([FromBody] VerifyEmailRequest verityRequest)
        {
            var result = await _mediator.Send(verityRequest);
            return HandleResult(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync([FromBody] LogoutRequest logoutRequest)
        {
            var result = await _mediator.Send(logoutRequest);
            return HandleResult(result);
        }

        [HttpDelete("delete-account")]
        public async Task<IActionResult> DeleteAccountAsync([FromBody] DeleteAccountRequest deleteAccountRequest)
        {
            var result = await _mediator.Send(deleteAccountRequest);
            return HandleResult(result);
        }
    }
}