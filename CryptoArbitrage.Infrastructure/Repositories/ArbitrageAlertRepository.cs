using CryptoArbitrage.Domain.Entities;
using CryptoArbitrage.Domain.Interfaces;
using CryptoArbitrage.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CryptoArbitrage.Infrastructure.Repositories
{
    public class ArbitrageAlertRepository : IArbitrageAlertRepository
    {
        private readonly AppDbContext _context;

        public ArbitrageAlertRepository(AppDbContext context)
        {
            _context = context;
        }

        // 1. Implementando o GetAllAsync (que o Controller usa)
        public async Task<IEnumerable<ArbitrageAlert>> GetAllAsync()
        {
            return await _context.ArbitrageAlerts
                .OrderByDescending(a => a.Id) // Se o Timestamp der erro, usamos o Id para ordenar
                .ToListAsync();
        }

        // 2. Implementando o GetRecentAlertsAsync (Corrigindo o Erro CS0535)
        public async Task<IEnumerable<ArbitrageAlert>> GetRecentAlertsAsync(int count)
        {
            return await _context.ArbitrageAlerts
                .OrderByDescending(a => a.Id) 
                .Take(count)
                .ToListAsync();
        }

        // 3. Método de adicionar (O que o Worker usa)
        public async Task AddAsync(ArbitrageAlert alert)
        {
            await _context.ArbitrageAlerts.AddAsync(alert);
            await _context.SaveChangesAsync();
        }
    }
}