namespace CryptoArbitrage.Application.DTOs;

public record ArbitrageResult(
    string Symbol,
    decimal PriceExA,
    decimal PriceExB,
    decimal Difference,
    decimal PercentageProfit
);