namespace Loner.Domain
{
    public class UserEntity : BaseEntity
    {
        public bool IsActive { get; set; }
        public bool IsVerifyAccount { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
