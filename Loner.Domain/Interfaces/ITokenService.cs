using System.Security.Claims;

namespace Loner.Domain.Interfaces
{
    public interface IJWTTokenService
    {
        Task<string> GenerateJwtAccessToken(UserEntity user);
        Task<string> GenerateJwtRefreshToken(UserEntity user);
        Task<bool> IsUserDeletedAsync(string userId);
        ClaimsPrincipal ValidateAccessToken(string accessToken);
    }
}