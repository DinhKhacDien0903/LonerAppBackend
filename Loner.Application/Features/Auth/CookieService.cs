using Loner.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Loner.Application.Features.Auth
{
    public class CookieService : ICookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CookieService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        public void SaveTokenToCookieHttpOnly(string name, string token, int expiresMinutes)
        {
            var cookieOption = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(expiresMinutes),
                SameSite = SameSiteMode.None,
            };
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null)
            {
                throw new Exception("HttpContext is not available.");
            }

            httpContext.Response.Cookies.Append(name, token, cookieOption);
        }

        public void RemoveTokenToCookieHttpOnly(string name)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null)
            {
                throw new Exception("HttpContext is not available.");
            }

            httpContext.Response.Cookies.Delete(name, new CookieOptions
            {
                SameSite = SameSiteMode.None,
                Secure = true,
                HttpOnly = true,
            });
        }

        public string GetTokenInCookies(string name)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null)
            {
                throw new Exception("HttpContext is not available.");
            }

            if (httpContext.Request.Cookies.TryGetValue(name, out var token))
            {
                return token;
            }

            throw new Exception("Token not found in cookies.");
        }
    }
}