using CryptoArbitrage.Application.DTOs;
using CryptoArbitrage.Application.Interfaces;
using CryptoArbitrage.Domain.Interfaces;

namespace CryptoArbitrage.Application.Services;

public class ArbitrageService : IArbitrageService
{
    private readonly IExternalPriceService _priceService;

    public ArbitrageService(IExternalPriceService priceService)
    {
        _priceService = priceService;
    }

    public async Task<ArbitrageResult> CalculateArbitrageAsync(string symbol)
    {
        // 1. Buscamos o preço na Binance (nossa Exchange A)
        var priceA = await _priceService.GetPriceAsync(symbol);

        // 2. Simulação: Como ainda não temos a Exchange B configurada, 
        // vamos simular um preço 2% menor para testar o cálculo.
        var priceB = priceA * 0.98m; 

        // 3. Cálculos de Arbitragem
        var difference = Math.Abs(priceA - priceB);
        var profitPercent = (difference / Math.Min(priceA, priceB)) * 100;

        return new ArbitrageResult(
            symbol.ToUpper(),
            priceA,
            priceB,
            difference,
            Math.Round(profitPercent, 2)
        );
    }
}