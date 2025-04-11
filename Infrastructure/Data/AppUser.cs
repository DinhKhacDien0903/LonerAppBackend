using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data
{
    public class AppUser : IdentityUser
    {
        public bool IsActive { get; set; }
        public bool IsVerifyAccount { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        public static UserEntity ToEntity(AppUser user)
        {
            return new UserEntity
            {
                AvatarUrl = user.AvatarUrl,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                IsVerifyAccount = user.IsVerifyAccount,
                TwoFactorEnabled = user.TwoFactorEnabled,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                PasswordHash = user.PasswordHash
            };
        }

        public static AppUser FromEntity(UserEntity user)
        {
            return new AppUser
            {
                AvatarUrl = user.AvatarUrl,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                IsVerifyAccount = user.IsVerifyAccount,
                TwoFactorEnabled = user.TwoFactorEnabled,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                PasswordHash = user.PasswordHash
            };
        }
    }
}