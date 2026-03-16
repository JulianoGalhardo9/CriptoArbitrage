using Microsoft.EntityFrameworkCore;
using CryptoArbitrage.Domain.Entities;

namespace CryptoArbitrage.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Cryptocurrency> Cryptocurrencies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Fluent API: Configurações detalhadas da tabela
        modelBuilder.Entity<Cryptocurrency>(entity => 
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Symbol).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Amount).HasPrecision(18, 8);
        });
    }
}