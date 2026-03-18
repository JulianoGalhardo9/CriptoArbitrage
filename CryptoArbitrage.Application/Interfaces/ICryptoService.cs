using CryptoArbitrage.Application.DTOs;

namespace CryptoArbitrage.Application.Interfaces;

public interface ICryptoService
{
    Task ExecuteAddAsync(RegisterCryptoRequest request);

    Task<PriceResponse> GetCurrentPriceAsync(string symbol);
}