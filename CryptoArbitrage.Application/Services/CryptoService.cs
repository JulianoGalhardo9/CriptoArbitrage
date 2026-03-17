using CryptoArbitrage.Application.DTOs;
using CryptoArbitrage.Application.Interfaces;
using CryptoArbitrage.Domain.Entities;
using CryptoArbitrage.Domain.Interfaces;

namespace CryptoArbitrage.Application.Services;

public class CryptoService : ICryptoService
{
    private readonly ICryptocurrencyRepository _repository;

    public CryptoService(ICryptocurrencyRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAddAsync(RegisterCryptoRequest request)
    {
        // Transformamos o DTO (dados brutos) em uma Entidade de Domínio
        var newCrypto = new Cryptocurrency(request.Symbol, request.Name, request.Amount);

        // Mandamos o repositório salvar essa entidade no banco
        await _repository.AddAsync(newCrypto);
    }
}