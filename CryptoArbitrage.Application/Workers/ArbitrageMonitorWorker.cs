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
                    var alertRepo = scope.ServiceProvider.GetRequiredService<IArbitrageAlertRepository>();
                    var cryptoRepo = scope.ServiceProvider.GetRequiredService<ICryptocurrencyRepository>();

                    // 1. Busca todas as moedas que você cadastrou (Ex: BTC, ETH, SOL)
                    var myCryptos = await cryptoRepo.GetAllAsync();

                    foreach (var crypto in myCryptos)
                    {
                        try
                        {
                            // O símbolo no banco pode ser "BTC", mas nas APIs precisamos de "BTCUSDT"
                            string symbolPair = crypto.Symbol.EndsWith("USDT") ? crypto.Symbol : $"{crypto.Symbol}USDT";

                            var result = await arbitrageService.CalculateArbitrageAsync(symbolPair);

                            if (strategy.IsProfitable(result.PercentageProfit))
                            {
                                // Salva o alerta...
                                _logger.LogWarning("💰 OPORTUNIDADE EM {Symbol}: {Profit}%", result.Symbol, result.PercentageProfit);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("❌ Erro ao processar {Symbol}: {Msg}", crypto.Symbol, ex.Message);
                        }

                        // Pequena pausa entre moedas para não ser bloqueado (Rate Limit)
                        await Task.Delay(500, stoppingToken);
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