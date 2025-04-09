using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace Infrastructure.Data
{
    internal class LonerDbContextFactory : IDesignTimeDbContextFactory<LonerDbContext>
    {
        public LonerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LonerDbContext>();
            var connectionString = "Server=localhost, 1433; Initial Catalog=Loner; Integrated Security=True; TrustServerCertificate=True";
            optionsBuilder.UseSqlServer(connectionString);

            return new LonerDbContext(optionsBuilder.Options);
        }
    }
}
