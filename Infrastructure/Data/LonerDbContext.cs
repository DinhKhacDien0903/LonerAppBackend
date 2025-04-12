using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class LonerDbContext : IdentityDbContext<UserEntity>
    {
        public LonerDbContext(DbContextOptions<LonerDbContext> options) : base(options) { }
        public DbSet<OTPEntity> OTPs { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RefreshTokenEntity>()
                .HasOne<UserEntity>(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}