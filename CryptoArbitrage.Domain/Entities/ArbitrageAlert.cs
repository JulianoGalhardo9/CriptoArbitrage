namespace CryptoArbitrage.Domain.Entities;

public class ArbitrageAlert
{
    public int Id { get; private set; }
    public string Symbol { get; private set; } = string.Empty;
    public decimal PriceBinance { get; private set; }
    public decimal PriceBitget { get; private set; }
    public decimal ProfitPercentage { get; private set; }
    public DateTime CreatedAt { get; private set; }
    private ArbitrageAlert() { }

    public ArbitrageAlert(string symbol, decimal priceBinance, decimal priceBitget, decimal profitPercentage)
    {
        Symbol = symbol;
        PriceBinance = priceBinance;
        PriceBitget = priceBitget;
        ProfitPercentage = profitPercentage;
        CreatedAt = DateTime.UtcNow;
    }
}