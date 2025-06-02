using Loner.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

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

            string? accessToken = "";
            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                accessToken = authHeader.Substring("Bearer ".Length).Trim();
            }

            if (string.IsNullOrEmpty(accessToken))
            {
                context.Request.Cookies.TryGetValue("access_token", out accessToken);
            }

            if (!string.IsNullOrEmpty(accessToken))
            {
                try
                {
                    var principal = authorService.ValidateAccessToken(accessToken);

                    if (principal != null)
                    {
                        bool isDeleted = await IsCurrentDeleted(principal, authorService);
                        if (!isDeleted)
                        {
                            context.User = principal;
                            await _next(context);
                            return;
                        }

                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsJsonAsync(new
                        {
                            error = "Tài khoản của bạn đã bị khóa.",
                            redirectToLogin = true
                        });

                        return;
                    }
                }
                catch (SecurityTokenValidationException e)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized; // Non-success (401)
                    await context.Response.WriteAsJsonAsync(new
                    {
                        error = "Invalid token",
                        redirectToLogin = true
                    });
                    return;
                }

            }

            await _next(context);
        }

        private async Task<bool> IsCurrentDeleted(ClaimsPrincipal principal, IJWTTokenService authorService)
        {
            try
            {
                bool isDeleted = false;
                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    isDeleted = await authorService.IsUserDeletedAsync(userId);
                }

                return isDeleted;
            }
            catch
            {
                return false;
            }
        }
    }
}