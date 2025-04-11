namespace Loner.Domain
{
    public class UserEntity : BaseEntity
    {
        public string Id { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsVerifyAccount { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string? AvatarUrl { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
    }
}