using Microsoft.AspNetCore.Identity;

namespace Loner.Domain
{
    public class UserEntity : IdentityUser
    {
        public bool IsActive { get; set; }
        public bool IsVerifyAccount { get; set; }
        public string? AvatarUrl { get; set; }
        [StringLength(255)]
        public string? About { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}