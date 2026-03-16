namespace CryptoArbitrage.Domain.Entities;

public class Cryptocurrency
{
    public int Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; } 
    public decimal AveragePurchasePrice { get; set; } 
    
    // Regra de negócio simples: Valor total investido nesta moeda
    public decimal TotalInvested => Amount * AveragePurchasePrice;
}