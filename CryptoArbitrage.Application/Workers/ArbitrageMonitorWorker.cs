using CryptoArbitrage.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
        _logger.LogInformation("🤖 Monitor de Arbitragem Iniciado...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Como este serviço roda "para sempre", precisamos criar um "escopo" 
                // para usar o ArbitrageService (que é Scoped)
                using (var scope = _serviceProvider.CreateScope())
                {
                    var arbitrageService = scope.ServiceProvider.GetRequiredService<IArbitrageService>();
                    
                    // Monitorando o par principal
                    var result = await arbitrageService.CalculateArbitrageAsync("BTCUSDT");

                    if (result.PercentageProfit > 0.5m) // Se o lucro for maior que 0.5%
                    {
                        _logger.LogWarning("🚀 OPORTUNIDADE: {Symbol} | Lucro: {Profit}% | Binance: {P1} | Bitget: {P2}", 
                            result.Symbol, result.PercentageProfit, result.PriceExA, result.PriceExB);
                    }
                    else
                    {
                        _logger.LogInformation("📊 Monitorando {Symbol}... Sem lucro relevante agora.", result.Symbol);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("❌ Erro no monitor: {Message}", ex.Message);
            }

            // Espera 10 segundos antes da próxima verificação
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}