using CryptoArbitrage.Domain.Interfaces;
using CryptoArbitrage.Infrastructure.Data;
using CryptoArbitrage.Infrastructure.ExternalServices;
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
            options.UseMySql(connectionString!, ServerVersion.AutoDetect(connectionString))); // Auto-detect é mais seguro para Docker

        // Repositórios
        services.AddScoped<ICryptocurrencyRepository, CryptocurrencyRepository>();
        services.AddScoped<IArbitrageAlertRepository, ArbitrageAlertRepository>();

        // Configuração do HttpClient com Resiliência (Timeout)
        services.AddHttpClient("CryptoClient", client =>
        {
            client.Timeout = TimeSpan.FromSeconds(5);
            // Adicionar um User-Agent ajuda a evitar bloqueios de algumas APIs
            client.DefaultRequestHeaders.Add("User-Agent", "CryptoArbitrageBot");
        });

        // Registro dos serviços concretos para o ArbitrageService (Cálculo Paralelo)
        services.AddScoped<BinanceService>();
        services.AddScoped<BitgetService>();

        // Interface padrão para outros serviços que precisem de um preço genérico
        services.AddScoped<IExternalPriceService, BinanceService>();

        return services;
    }
}