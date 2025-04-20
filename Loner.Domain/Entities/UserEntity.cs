using Microsoft.AspNetCore.Identity;

namespace Loner.Domain
{
    public class UserEntity : IdentityUser
    {
        public bool IsActive { get; set; }
        public bool IsVerifyAccount { get; set; }
        public string AvatarUrl { get; set; } = string.Empty;
        [StringLength(255)]
        public string? About { get; set; }
        public string? Address { get; set; }
        public string? University { get; set; }
        public string? Work { get; set; }
        public bool IsDeleted { get; set; }
        public bool Gender { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Age { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}