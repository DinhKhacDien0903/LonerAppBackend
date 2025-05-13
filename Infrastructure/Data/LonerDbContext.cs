using Loner.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class LonerDbContext : IdentityDbContext<UserEntity>
    {
        public LonerDbContext(DbContextOptions<LonerDbContext> options) : base(options) { }
        public DbSet<OTPEntity> OTP { get; set; }
        public DbSet<RefreshTokenEntity> RefreshToken { get; set; }
        public DbSet<InterestEntity> Interest { get; set; }
        public DbSet<MatchesEntity> Matche { get; set; }
        public DbSet<MessageEntity> Message { get; set; }
        public DbSet<NotificationEntity> Notification { get; set; }
        public DbSet<PreferenceEntity> Preference { get; set; }
        public DbSet<Preference_InterestEntity> Preference_Interest { get; set; }
        public DbSet<PhotoEntity> Photo { get; set; }
        public DbSet<SwipeEntity> Swipe { get; set; }
        public DbSet<User_InterestEntity> User_Interest { get; set; }
        public DbSet<ReportEntity> Report { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RefreshTokenEntity>()
                .HasOne<UserEntity>(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MatchesEntity>()
                .HasOne<UserEntity>(m => m.User1)
                .WithMany()
                .HasForeignKey(m => m.User1Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<MatchesEntity>()
                .HasOne<UserEntity>(m => m.User2)
                .WithMany()
                .HasForeignKey(m => m.User2Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<SwipeEntity>()
                .HasOne<UserEntity>(m => m.Swiper)
                .WithMany()
                .HasForeignKey(m => m.SwiperId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<SwipeEntity>()
                .HasOne<UserEntity>(m => m.Swiped)
                .WithMany()
                .HasForeignKey(m => m.SwipedId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Preference_InterestEntity>()
                .HasOne<PreferenceEntity>(m => m.Preference)
                .WithMany()
                .HasForeignKey(m => m.PreferenceId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Preference_InterestEntity>()
                .HasOne<InterestEntity>(m => m.Interest)
                .WithMany()
                .HasForeignKey(m => m.InterestId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<User_InterestEntity>()
                .HasOne<UserEntity>(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<User_InterestEntity>()
                .HasOne<InterestEntity>(m => m.Interest)
                .WithMany()
                .HasForeignKey(m => m.InterestId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<MessageEntity>()
                .HasOne<MatchesEntity>(m => m.Matches)
                .WithMany()
                .HasForeignKey(m => m.MatchId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<MessageEntity>()
                .HasOne<UserEntity>(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ReportEntity>()
                .HasOne<UserEntity>(m => m.Reporter)
                .WithMany()
                .HasForeignKey(m => m.ReporterId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}