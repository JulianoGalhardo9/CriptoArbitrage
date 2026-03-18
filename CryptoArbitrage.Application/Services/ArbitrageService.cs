using CryptoArbitrage.Application.DTOs;
using CryptoArbitrage.Application.Interfaces;
using CryptoArbitrage.Infrastructure.ExternalServices;

public class ArbitrageService : IArbitrageService
{
    private readonly BinanceService _binance;
    private readonly BitgetService _bitget;

    // Injetamos as implementações específicas diretamente
    public ArbitrageService(BinanceService binance, BitgetService bitget)
    {
        _binance = binance;
        _bitget = bitget;
    }

    public async Task<ArbitrageResult> CalculateArbitrageAsync(string symbol)
    {
        // Agora buscamos preços REAIS em dois lugares diferentes ao mesmo tempo!
        var priceBinance = await _binance.GetPriceAsync(symbol);
        var priceBitget = await _bitget.GetPriceAsync(symbol);

        if (priceBinance == 0 || priceBitget == 0)
            throw new Exception("Erro ao buscar preços em uma das exchanges.");

        var difference = Math.Abs(priceBinance - priceBitget);
        var profitPercent = (difference / Math.Min(priceBinance, priceBitget)) * 100;

        return new ArbitrageResult(
            symbol.ToUpper(),
            priceBinance, // Preço Exchange A
            priceBitget,  // Preço Exchange B
            difference,
            Math.Round(profitPercent, 2)
        );
    }
}