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
        // Estratégia agressiva para teste (mude para valores reais depois)
        var strategy = new ArbitrageStrategy { MinProfitPercentage = 0.3m, EstimatedFees = 0.15m };

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
                        // Cada moeda em seu próprio escopo para evitar conflitos no DbContext
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            try
                            {
                                var arbitrageService = scope.ServiceProvider.GetRequiredService<IArbitrageService>();
                                var alertRepo = scope.ServiceProvider.GetRequiredService<IArbitrageAlertRepository>();
                                var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                                string symbolPair = crypto.Symbol.EndsWith("USDT") ? crypto.Symbol : $"{crypto.Symbol}USDT";
                                var result = await arbitrageService.CalculateArbitrageAsync(symbolPair);

                                // TESTE DE FORÇA BRUTA (if true envia sempre)
                                if (strategy.IsProfitable(result.PercentageProfit))
                                {
                                    var alert = new ArbitrageAlert(result.Symbol, result.PriceExA, result.PriceExB, result.PercentageProfit);
                                    await alertRepo.AddAsync(alert);

                                    _logger.LogWarning("🚀 TESTE DE ENVIO PARA {Symbol}: {Profit}%", result.Symbol, result.PercentageProfit);

                                    var message = $"ANÁLISE DO ROBÔ!\nMoeda: {result.Symbol}\nLucro Atual: {result.PercentageProfit}%\nBinance: ${result.PriceExA}\nBitget: ${result.PriceExB}";

                                    await notificationService.SendAlertAsync(message);

                                    // Delay para não sobrecarregar a API do Telegram
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

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}