using Microsoft.Extensions.Hosting;

namespace DemocracyBot.Integration.Telegram.Abstractions
{
    public interface IUserTelegramService : IHostedService
    {
        WTelegram.Client Client { get; set; }
    }
}