using System.Text.Json;
using CryptoArbitrage.Domain.Interfaces;

namespace CryptoArbitrage.Infrastructure.ExternalServices;

public class BitgetService : IExternalPriceService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BitgetService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<decimal> GetPriceAsync(string symbol)
    {
        var client = _httpClientFactory.CreateClient();
        
        // URL da Bitget para Ticker (Preço atual)
        var url = $"https://api.bitget.com/api/v2/spot/market/tickers?symbol={symbol.ToUpper()}";

        var response = await client.GetAsync(url);
        
        if (!response.IsSuccessStatusCode) return 0;

        var content = await response.Content.ReadAsStringAsync();
        using var jsonDoc = JsonDocument.Parse(content);
        
        // A Bitget retorna um array dentro de "data". Pegamos o primeiro item [0]
        var dataElement = jsonDoc.RootElement.GetProperty("data")[0];
        var priceString = dataElement.GetProperty("lastPr").GetString();

        return decimal.Parse(priceString ?? "0", System.Globalization.CultureInfo.InvariantCulture);
    }
}