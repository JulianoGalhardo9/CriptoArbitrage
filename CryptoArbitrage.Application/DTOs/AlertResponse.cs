namespace CryptoArbitrage.Application.DTOs;

public record AlertResponse(
    string Symbol, 
    decimal PriceBinance, 
    decimal PriceBitget, 
    decimal Profit, 
    DateTime CreatedAt);