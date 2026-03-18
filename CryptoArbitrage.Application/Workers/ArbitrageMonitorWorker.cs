using CryptoArbitrage.Application.Interfaces;
using CryptoArbitrage.Domain.Entities;
using CryptoArbitrage.Domain.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

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
                    if (result.PercentageProfit > 0.2m)
                    {
                        _logger.LogWarning("OPORTUNIDADE ENCONTRADA: {Symbol} | Lucro: {Profit}%", 
                            result.Symbol, result.PercentageProfit);

                        // 3. Persiste a oportunidade no banco de dados
                        var newAlert = new ArbitrageAlert(
                            result.Symbol, 
                            result.PriceExA, 
                            result.PriceExB, 
                            result.PercentageProfit
                        );

                        await alertRepository.AddAsync(newAlert);
                        _logger.LogInformation("Alerta salvo no banco de dados com sucesso.");
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