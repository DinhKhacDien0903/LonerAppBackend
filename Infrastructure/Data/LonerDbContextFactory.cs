using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data
{
    internal class LonerDbContextFactory : IDesignTimeDbContextFactory<LonerDbContext>
    {
        public LonerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LonerDbContext>();
            var connectionString = Enviroments.ConnectionString_SSMS;
            optionsBuilder.UseSqlServer(connectionString);

            return new LonerDbContext(optionsBuilder.Options);
        }
    }
}