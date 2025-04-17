using static Loner.Application.DTOs.Auth;
using Swashbuckle.AspNetCore.Filters;

namespace Loner.Presentation.SwaggerDataExample
{
    public class VerifyEmailRequestExample : IExamplesProvider<VerifyEmailRequest>
    {
        public VerifyEmailRequest GetExamples()
        {
            return new VerifyEmailRequest(
                Email: "user4@test.com",
                Otp: "123456",
                IsLoggingIn: true
            );
        }
    }
}