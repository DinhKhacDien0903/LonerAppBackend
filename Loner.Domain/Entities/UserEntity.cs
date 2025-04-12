using Microsoft.AspNetCore.Identity;

namespace Loner.Domain
{
    public class UserEntity : IdentityUser
    {
        public bool IsActive { get; set; }
        public bool IsVerifyAccount { get; set; }
        public string? AvatarUrl { get; set; }
        public bool IsDeleted { get; set; }
    }
}