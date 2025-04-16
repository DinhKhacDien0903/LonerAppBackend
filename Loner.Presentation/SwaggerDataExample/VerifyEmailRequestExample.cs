using static Loner.Application.DTOs.Auth;
using Swashbuckle.AspNetCore.Filters;

namespace Loner.Presentation.SwaggerDataExample
{
    public class VerifyEmailRequestExample : IExamplesProvider<VerifyEmailRequest>
    {
        public VerifyEmailRequest GetExamples()
        {
            return new VerifyEmailRequest(
                Email: "everyonepiano107@gmail.com",
                Otp: "873168",
                IsLoggingIn: true
            );
        }
    }
}