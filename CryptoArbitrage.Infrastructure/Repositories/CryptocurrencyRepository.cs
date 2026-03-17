using CryptoArbitrage.Domain.Entities;
using CryptoArbitrage.Domain.Interfaces;
using CryptoArbitrage.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CryptoArbitrage.Infrastructure.Repositories;

public class CryptocurrencyRepository : ICryptocurrencyRepository
{
    private readonly AppDbContext _context;

    // Injeção de Dependência do nosso Contexto de Banco
    public CryptocurrencyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Cryptocurrency crypto)
    {
        await _context.Cryptocurrencies.AddAsync(crypto);
        await _context.SaveChangesAsync(); // Salva de fato no MySQL
    }

    public async Task<IEnumerable<Cryptocurrency>> GetAllAsync()
    {
        // AsNoTracking melhora a performance em consultas de apenas leitura
        return await _context.Cryptocurrencies.AsNoTracking().ToListAsync();
    }

    public async Task<Cryptocurrency?> GetByIdAsync(int id)
    {
        return await _context.Cryptocurrencies.FindAsync(id);
    }

    public void Update(Cryptocurrency crypto)
    {
        _context.Cryptocurrencies.Update(crypto);
        _context.SaveChanges();
    }

    public async Task DeleteAsync(int id)
    {
        var crypto = await GetByIdAsync(id);
        if (crypto != null)
        {
            _context.Cryptocurrencies.Remove(crypto);
            await _context.SaveChangesAsync();
        }
    }
}