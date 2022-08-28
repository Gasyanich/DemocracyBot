using System.Threading;
using System.Threading.Tasks;
using DemocracyBot.Integration.Telegram.Abstractions;
using WTelegram;

namespace DemocracyBot.LongPolling
{
    public class UserTelegramService : IUserTelegramService
    {
        public Client Client { get; set; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
        }
    }
}