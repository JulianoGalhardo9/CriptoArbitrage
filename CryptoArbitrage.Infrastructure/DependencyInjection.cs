using CryptoArbitrage.Domain.Interfaces;
using CryptoArbitrage.Infrastructure.Data;
using CryptoArbitrage.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoArbitrage.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString!, new MariaDbServerVersion(new Version(12, 2, 2))));

        services.AddScoped<ICryptocurrencyRepository, CryptocurrencyRepository>();
        return services;
    }
}