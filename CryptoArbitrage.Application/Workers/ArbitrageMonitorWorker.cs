using CryptoArbitrage.Application.Interfaces;
using CryptoArbitrage.Domain.Entities;
using CryptoArbitrage.Domain.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using CryptoArbitrage.Domain.Models;

namespace CryptoArbitrage.Application.Workers;

public class ArbitrageMonitorWorker : BackgroundService
{
    private readonly ILogger<ArbitrageMonitorWorker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ArbitrageMonitorWorker(ILogger<ArbitrageMonitorWorker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Definimos a estratégia (no futuro, isso pode vir do appsettings.json ou do Banco)
        var strategy = new ArbitrageStrategy { MinProfitPercentage = 0.3m, EstimatedFees = 0.15m };

        _logger.LogInformation("🤖 Monitor de Arbitragem iniciado e vigiando o mercado...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Criamos um escopo temporário para usar serviços "Scoped" (Banco e APIs)
                using (var scope = _serviceProvider.CreateScope())
                {
                    var arbitrageService = scope.ServiceProvider.GetRequiredService<IArbitrageService>();
                    var alertRepository = scope.ServiceProvider.GetRequiredService<IArbitrageAlertRepository>();

                    // 1. Calcula a diferença entre Binance e Bitget
                    var result = await arbitrageService.CalculateArbitrageAsync("BTCUSDT");

                    // 2. Lógica de Alerta: Se o lucro for maior que 0.2% (ajustável)
                    if (strategy.IsProfitable(result.PercentageProfit))
                    {
                        _logger.LogWarning("LUCRO REAL DETECTADO (Descontando Taxas): {Profit}%",
                            result.PercentageProfit - strategy.EstimatedFees);
                    }
                    else
                    {
                        _logger.LogInformation("{Time} - {Symbol}: {Profit}% (Aguardando oportunidade...)",
                            DateTime.Now.ToString("HH:mm:ss"), result.Symbol, result.PercentageProfit);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro no ciclo do monitor: {Message}", ex.Message);
            }

            // Espera 10 segundos para não ser bloqueado pelas APIs (Rate Limit)
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}