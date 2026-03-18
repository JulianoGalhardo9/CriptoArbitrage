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
        // Configuração: Lucro alvo de 0.5% já descontando 0.15% de taxas estimadas
        var strategy = new ArbitrageStrategy { MinProfitPercentage = 5.15m, EstimatedFees = 0.15m };

        _logger.LogInformation("🤖 Monitor de Arbitragem iniciado e vigiando o mercado...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var initialScope = _serviceProvider.CreateScope())
                {
                    var cryptoRepo = initialScope.ServiceProvider.GetRequiredService<ICryptocurrencyRepository>();
                    var myCryptos = await cryptoRepo.GetAllAsync();

                    var tasks = myCryptos.Select(async crypto =>
                    {
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            try
                            {
                                var arbitrageService = scope.ServiceProvider.GetRequiredService<IArbitrageService>();
                                var alertRepo = scope.ServiceProvider.GetRequiredService<IArbitrageAlertRepository>();
                                var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                                string symbolPair = crypto.Symbol.EndsWith("USDT") ? crypto.Symbol : $"{crypto.Symbol}USDT";
                                var result = await arbitrageService.CalculateArbitrageAsync(symbolPair);

                                // Lógica de Lucro Real
                                if (strategy.IsProfitable(result.PercentageProfit))
                                {
                                    var alert = new ArbitrageAlert(result.Symbol, result.PriceExA, result.PriceExB, result.PercentageProfit);
                                    await alertRepo.AddAsync(alert);

                                    _logger.LogWarning("💰 LUCRO DETECTADO EM {Symbol}: {Profit}%", result.Symbol, result.PercentageProfit);

                                    var message = $"OPORTUNIDADE DE LUCRO!\n" +
                                                  $"Moeda: {result.Symbol}\n" +
                                                  $"Lucro: {result.PercentageProfit}%\n" +
                                                  $"Binance: ${result.PriceExA}\n" +
                                                  $"Bitget: ${result.PriceExB}";

                                    // Usamos a variável já declarada acima, sem o 'var'
                                    await notificationService.SendAlertAsync(message);

                                    await Task.Delay(1000, stoppingToken);
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError("❌ Falha ao processar {Symbol}: {Msg}", crypto.Symbol, ex.Message);
                            }
                        }
                    });

                    await Task.WhenAll(tasks);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("⚠️ Erro no ciclo do monitor: {Message}", ex.Message);
            }

            // Aguarda 10 segundos para a próxima varredura
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}