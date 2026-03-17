using CryptoArbitrage.Domain.Entities;

namespace CryptoArbitrage.Domain.Interfaces;

public interface ICryptocurrencyRepository
{
    Task AddAsync(Cryptocurrency crypto);
    Task<IEnumerable<Cryptocurrency>> GetAllAsync();
    Task<Cryptocurrency?> GetByIdAsync(int id);
    void Update(Cryptocurrency crypto);
    Task DeleteAsync(int id);
}