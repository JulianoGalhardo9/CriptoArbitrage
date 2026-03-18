namespace CryptoArbitrage.Domain.Models;

public class ArbitrageStrategy
{
    public decimal MinProfitPercentage { get; set; } = 0.5m; // Lucro mínimo para alertar
    public decimal EstimatedFees { get; set; } = 0.2m;      // Taxas estimadas (Binance + Bitget)
    
    public bool IsProfitable(decimal currentProfit) 
    {
        return currentProfit > (MinProfitPercentage + EstimatedFees);
    }
}