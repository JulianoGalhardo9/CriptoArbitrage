using CryptoArbitrage.Application.DTOs;
using CryptoArbitrage.Application.Interfaces;
using CryptoArbitrage.Domain.Entities;
using CryptoArbitrage.Domain.Interfaces;

namespace CryptoArbitrage.Application.Services;

public class CryptoService : ICryptoService
{
    private readonly ICryptocurrencyRepository _repository;
    private readonly IExternalPriceService _priceService;

    public CryptoService(ICryptocurrencyRepository repository, IExternalPriceService priceService)
    {
        _repository = repository;
        _priceService = priceService;
    }

    public async Task ExecuteAddAsync(RegisterCryptoRequest request)
    {
        // Transformamos o DTO (dados brutos) em uma Entidade de Domínio
        var newCrypto = new Cryptocurrency(request.Symbol, request.Name, request.Amount);

        // Mandamos o repositório salvar essa entidade no banco
        await _repository.AddAsync(newCrypto);
    }

    public async Task<PriceResponse> GetCurrentPriceAsync(string symbol)
    {
        // 1. Chama o serviço da Binance que criamos no passo passado
        var price = await _priceService.GetPriceAsync(symbol);

        // 2. Monta a resposta para a API
        return new PriceResponse(symbol.ToUpper(), price, DateTime.UtcNow);
    }
}