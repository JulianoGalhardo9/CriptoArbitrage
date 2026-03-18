namespace CryptoArbitrage.Domain.Interfaces;

public interface IExternalPriceService
{
    // Task<decimal>: Promessa de retornar um número decimal (o preço)
    // symbol: O código da moeda (ex: BTCUSDT)
    Task<decimal> GetPriceAsync(string symbol);
}