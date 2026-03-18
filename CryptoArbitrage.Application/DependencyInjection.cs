using CryptoArbitrage.Application.Interfaces;
using CryptoArbitrage.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoArbitrage.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICryptoService, CryptoService>();
        services.AddScoped<IArbitrageService, ArbitrageService>();

        return services;
    }
}