namespace Loner.Domain
{
    public class UserEntity : BaseEntity
    {
        public bool IsActive { get; set; }
        public bool IsVerifyAccount { get; set; }
        public string? AvatarUrl { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
    }
}