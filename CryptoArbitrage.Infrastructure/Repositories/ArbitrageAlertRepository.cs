using CryptoArbitrage.Domain.Entities;
using CryptoArbitrage.Domain.Interfaces;
using CryptoArbitrage.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CryptoArbitrage.Infrastructure.Repositories;

public class ArbitrageAlertRepository : IArbitrageAlertRepository
{
    private readonly AppDbContext _context;
    public ArbitrageAlertRepository(AppDbContext context) => _context = context;

    public async Task AddAsync(ArbitrageAlert alert)
    {
        await _context.ArbitrageAlerts.AddAsync(alert);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ArbitrageAlert>> GetRecentAlertsAsync(int count)
    {
        return await _context.ArbitrageAlerts
            .OrderByDescending(a => a.CreatedAt)
            .Take(count)
            .ToListAsync();
    }
}