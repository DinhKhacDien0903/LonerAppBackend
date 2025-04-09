using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class LonerDbContext : IdentityDbContext<AppUser>
    {
        public LonerDbContext(DbContextOptions<LonerDbContext> options) : base(options) { }
    }
}
