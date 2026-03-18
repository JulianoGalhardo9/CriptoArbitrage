using CryptoArbitrage.Domain.Entities;
namespace CryptoArbitrage.Domain.Interfaces;

public interface IArbitrageAlertRepository
{
    Task AddAsync(ArbitrageAlert alert);
    Task<IEnumerable<ArbitrageAlert>> GetRecentAlertsAsync(int count);
}