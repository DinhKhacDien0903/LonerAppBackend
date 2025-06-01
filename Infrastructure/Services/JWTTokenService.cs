using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services
{
    public class JWTTokenService : IJWTTokenService
    {
        private readonly JwtConfig _jwtConfig = new();
        private readonly UserManager<UserEntity>? _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public JWTTokenService(
            IOptionsMonitor<JwtConfig> config,
            UserManager<UserEntity>? userManager,
            IUnitOfWork unitOfWork)
        {
            _jwtConfig = config.CurrentValue;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> GenerateJwtAccessToken(UserEntity user)
        {
            var secretKey = Encoding.UTF8.GetBytes(_jwtConfig.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("ID", Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                    new Claim(JwtRegisteredClaimNames.PhoneNumber, user.PhoneNumber ?? ""),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),

                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience,
                Expires = DateTime.UtcNow.AddDays(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256)
            };

            if(_userManager != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                foreach (var role in userRoles)
                {
                    tokenDescription.Subject.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
                }
            }

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(token);
        }

        public async Task<string> GenerateJwtRefreshToken(UserEntity user)
        {
            await Task.Delay(1);
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }

        public async Task<bool> IsUserDeletedAsync(string userId)
        {
            return await _unitOfWork.UserRepository.IsUserDeletedAsync(userId);
        }

        public ClaimsPrincipal ValidateAccessToken(string accessToken)
        {
            try
            {
                var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

                var screteKeyBytes = Encoding.UTF8.GetBytes(_jwtConfig.SecretKey);

                var tokenValidateParamater = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(screteKeyBytes),
                    ClockSkew = TimeSpan.Zero,
                };

                return tokenHandler.ValidateToken(accessToken, tokenValidateParamater, out var validatedToken);

            }
            catch (Exception e)
            {
                throw new SecurityTokenValidationException("Token is not valid", e);
            }
        }
    }
}