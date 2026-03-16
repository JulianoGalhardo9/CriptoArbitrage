namespace CryptoArbitrage.Domain.Entities;

public class Cryptocurrency
{
    public int Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal AveragePurchasePrice { get; set; }
    public DateTime LastUpdate { get; set; }

    public Cryptocurrency(string symbol, string name, decimal amount)
    {
        Symbol = symbol.ToUpper();
        Name = name;
        Amount = amount;
        LastUpdate = DateTime.UtcNow;
    }
    public void UpdateAmount(decimal newAmount)
    {
        if (newAmount < 0) throw new Exception("Amount cannot be negative");
        Amount = newAmount;
        LastUpdate = DateTime.UtcNow;
    }
}