using System.Threading.Tasks;
using DemocracyBot.Domain.Commands.Abstractions;
using Telegram.Bot;

namespace DemocracyBot.Domain.Commands.Commands
{
    [Command("meet")]
    public class MeetCommand : CommandBase
    {
        public MeetCommand(TelegramBotClient client) : base(client)
        {
        }

        public async override Task Execute()
        {
        }
    }
}