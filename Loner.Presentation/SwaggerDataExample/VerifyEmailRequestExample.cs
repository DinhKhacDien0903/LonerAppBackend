using static Loner.Application.DTOs.Auth;
using Swashbuckle.AspNetCore.Filters;

namespace Loner.Presentation.SwaggerDataExample
{
    public class VerifyEmailRequestExample : IExamplesProvider<VerifyEmailRequest>
    {
        public VerifyEmailRequest GetExamples()
        {
            return new VerifyEmailRequest(
                Email: "dinhkhacdien03@gmail.com",
                Otp: "918387",
                IsLoggingIn: true
            );
        }
    }
}