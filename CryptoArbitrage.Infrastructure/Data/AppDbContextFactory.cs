using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CryptoArbitrage.Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseMySql(
            "Server=127.0.0.1;Port=3306;Database=CryptoDb;User=root;Password=root_password;",
            new MariaDbServerVersion(new Version(12, 2, 2))
        );
        return new AppDbContext(optionsBuilder.Options);
    }
}
