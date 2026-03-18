namespace CryptoArbitrage.Domain.Interfaces;

public interface INotificationService
{
    /// <summary>
    /// Envia uma mensagem de alerta para o canal configurado (ex: Telegram).
    /// </summary>
    /// <param name="message">O texto da mensagem formatado.</param>
    Task SendAlertAsync(string message);
}