using System.Text.Json;
using CryptoArbitrage.Domain.Interfaces;

namespace CryptoArbitrage.Infrastructure.ExternalServices;

public class BinanceService : IExternalPriceService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BinanceService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<decimal> GetPriceAsync(string symbol)
    {
        // 1. Criamos um "cliente" HTTP (como se fosse um navegador invisível)
        var client = _httpClientFactory.CreateClient();
        
        // 2. Montamos a URL da API pública da Binance
        var url = $"https://api.binance.com/api/v3/ticker/price?symbol={symbol.ToUpper()}";

        // 3. Fazemos a requisição e esperamos a resposta
        var response = await client.GetAsync(url);
        
        // Se a Binance responder erro (moeda inexistente), lançamos uma exceção
        if (!response.IsSuccessStatusCode)
            return 0;

        // 4. Lemos o conteúdo da resposta (JSON)
        var content = await response.Content.ReadAsStringAsync();
        
        // 5. Extraímos apenas o valor do campo "price" do JSON
        using var jsonDoc = JsonDocument.Parse(content);
        var priceString = jsonDoc.RootElement.GetProperty("price").GetString();

        return decimal.Parse(priceString ?? "0", System.Globalization.CultureInfo.InvariantCulture);
    }
}