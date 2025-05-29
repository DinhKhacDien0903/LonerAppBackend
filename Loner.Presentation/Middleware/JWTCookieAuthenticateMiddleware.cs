using Loner.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Loner.Presentation.Middleware
{
    public class JWTCookieAuthenticateMiddleware
    {
        private readonly RequestDelegate _next;
        public JWTCookieAuthenticateMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
        {
            var authorService = serviceProvider.GetRequiredService<IJWTTokenService>();

            var accessToken = context.Request.Cookies["access_token"];

            if (!string.IsNullOrEmpty(accessToken))
            {
                try
                {
                    var principal = authorService.ValidateAccessToken(accessToken);

                    if (principal != null)
                    {
                        context.User = principal;
                    }

                }
                catch (SecurityTokenValidationException e)
                {
                    //todo => write logg
                }

            }
            await _next(context);
        }
    }
}
