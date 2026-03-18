using CryptoArbitrage.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CryptoArbitrage.Infrastructure.Notifications;

public class TelegramNotificationService : INotificationService
{
    private readonly HttpClient _httpClient;
    private readonly string _botToken;
    private readonly string _chatId;

    public TelegramNotificationService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _botToken = configuration["Telegram:BotToken"] ?? string.Empty;
        _chatId = configuration["Telegram:ChatId"] ?? string.Empty;

        Console.WriteLine($"DEBUG: Iniciando Telegram com ID: {_chatId}");
    }

    public async Task SendAlertAsync(string message)
    {
        if (string.IsNullOrWhiteSpace(_botToken) || string.IsNullOrWhiteSpace(_chatId))
        {
            Console.WriteLine("⚠️ Alerta: Configurações do Telegram ausentes no appsettings.json.");
            return;
        }

        var url = $"https://api.telegram.org/bot{_botToken}/sendMessage?chat_id={_chatId}&text={Uri.EscapeDataString(message)}";
        
        try 
        {
            var response = await _httpClient.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("✅ Notificação enviada ao Telegram!");
            }
            else 
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ Erro na API do Telegram: {response.StatusCode} - {error}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erro de conexão ao enviar Telegram: {ex.Message}");
        }
    }
}