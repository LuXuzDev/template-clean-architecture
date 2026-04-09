namespace Application.Services.PersonalLoggerNotifier.Telegram;

public class TelegramSettings
{
    public string BotToken { get; set; } = string.Empty;
    public List<string> ChatIds { get; set; } = new();
}