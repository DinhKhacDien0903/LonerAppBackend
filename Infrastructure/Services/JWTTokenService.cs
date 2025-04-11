
using AutoMapper;
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
        private readonly UserManager<AppUser>? _userManager;

        public JWTTokenService(
            IOptionsMonitor<JwtConfig> config,
            UserManager<AppUser>? userManager)
        {
            _jwtConfig = config.CurrentValue;
            _userManager = userManager;
        }

        public async Task<string> GenerateJwtAccessToken(UserEntity user)
        {
            var appUser = AppUser.FromEntity(user);
            var secretKey = Encoding.UTF8.GetBytes(_jwtConfig.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("ID", Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, appUser.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, appUser.Email ?? ""),
                    new Claim(JwtRegisteredClaimNames.PhoneNumber, appUser.PhoneNumber ?? ""),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),

                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience,
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256)
            };

            if(_userManager != null)
            {
                var userRoles = await _userManager.GetRolesAsync(appUser);

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
    }
}