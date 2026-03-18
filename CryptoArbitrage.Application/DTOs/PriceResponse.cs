namespace CryptoArbitrage.Application.DTOs;

public record PriceResponse(string Symbol, decimal Price, DateTime RetrievedAt);