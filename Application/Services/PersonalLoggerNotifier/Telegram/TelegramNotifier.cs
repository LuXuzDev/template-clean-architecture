using Loop.PersonalLogger;

namespace Application.Services.PersonalLoggerNotifier.Telegram;


public class TelegramNotifier : IExternalNotifier
{
    private readonly string _botToken;
    private readonly List<string> _chatIds;
    private readonly HttpClient _httpClient;

    public TelegramNotifier(string botToken, IEnumerable<string> chatIds)
    {
        _botToken = botToken;
        _chatIds = chatIds.ToList();
        _httpClient = new HttpClient();
    }

    public async Task NotifyAsync(string message)
    {
        var tasks = _chatIds.Select(chatId =>
        {
            var url = $"https://api.telegram.org/bot{_botToken}/sendMessage";

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "chat_id", chatId },
                { "text", message }
            });

            return _httpClient.PostAsync(url, content);
        });

        await Task.WhenAll(tasks);
    }
}