using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class LonerDbContext : IdentityDbContext<AppUser>
    {
        public LonerDbContext(DbContextOptions<LonerDbContext> options) : base(options) { }
    }
}