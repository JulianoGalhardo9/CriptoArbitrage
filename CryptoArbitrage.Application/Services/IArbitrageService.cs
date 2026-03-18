using CryptoArbitrage.Application.DTOs;

namespace CryptoArbitrage.Application.Interfaces;

public interface IArbitrageService
{
    Task<ArbitrageResult> CalculateArbitrageAsync(string symbol);
}