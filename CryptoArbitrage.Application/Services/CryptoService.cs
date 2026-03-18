using CryptoArbitrage.Application.DTOs;
using CryptoArbitrage.Application.Interfaces;
using CryptoArbitrage.Domain.Entities;
using CryptoArbitrage.Domain.Interfaces;

namespace CryptoArbitrage.Application.Services;
public class CryptoService : ICryptoService
{
    private readonly ICryptocurrencyRepository _repository;
    private readonly IExternalPriceService _priceService;
    private readonly IArbitrageAlertRepository _alertRepository;

    public CryptoService(ICryptocurrencyRepository repository, IExternalPriceService priceService, IArbitrageAlertRepository alertRepository)
    {
        _repository = repository;
        _priceService = priceService;
        _alertRepository = alertRepository;
    }

    public async Task ExecuteAddAsync(RegisterCryptoRequest request)
    {
        var newCrypto = new Cryptocurrency(request.Symbol, request.Name, request.Amount);
        await _repository.AddAsync(newCrypto);
    }

    public async Task<PriceResponse> GetCurrentPriceAsync(string symbol)
    {
        var price = await _priceService.GetPriceAsync(symbol);
        return new PriceResponse(symbol.ToUpper(), price, DateTime.UtcNow);
    }

    public async Task<IEnumerable<AlertResponse>> GetRecentAlertsAsync(int count)
    {
        var alerts = await _alertRepository.GetRecentAlertsAsync(count);
        return alerts.Select(a => new AlertResponse(
            a.Symbol, a.PriceBinance, a.PriceBitget, a.ProfitPercentage, a.CreatedAt));
    }
}