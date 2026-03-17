namespace CryptoArbitrage.Application.DTOs;

public class RegisterCryptoRequest
{
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}