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

                    // Busca todas as moedas que você cadastrou (Ex: BTC, ETH, SOL)
                    var myCryptos = await cryptoRepo.GetAllAsync();

                    // Criamos uma lista de tarefas (Tasks) para todas as moedas
                    var tasks = myCryptos.Select(async crypto =>
                    {
                        try
                        {
                            string symbolPair = crypto.Symbol.EndsWith("USDT") ? crypto.Symbol : $"{crypto.Symbol}USDT";

                            // Chamada assíncrona paralela
                            var result = await arbitrageService.CalculateArbitrageAsync(symbolPair);

                            if (strategy.IsProfitable(result.PercentageProfit))
                            {
                                var alert = new ArbitrageAlert(result.Symbol, result.PriceExA, result.PriceExB, result.PercentageProfit);
                                await alertRepo.AddAsync(alert);
                                _logger.LogWarning("💰 OPORTUNIDADE EM {Symbol}: {Profit}%", result.Symbol, result.PercentageProfit);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(" Falha ao processar {Symbol}: {Msg}", crypto.Symbol, ex.Message);
                        }
                    });

                    // Executa todas as consultas simultaneamente
                    await Task.WhenAll(tasks);
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