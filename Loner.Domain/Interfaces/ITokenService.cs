namespace Loner.Domain.Interfaces
{
    public interface IJWTTokenService
    {
        Task<string> GenerateJwtAccessToken(UserEntity user);
        Task<string> GenerateJwtRefreshToken(UserEntity user);
    }
}