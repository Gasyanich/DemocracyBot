using System.Threading.Tasks;
using DemocracyBot.DataAccess.Repository.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions.CommandsBase;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DemocracyBot.Domain.Commands.Commands
{
    [Command("stop")]
    public class StopCommand : MessageCommandBase
    {
        private readonly IChatRepository _chatRepository;

        public StopCommand(TelegramBotClient client, IChatRepository chatRepository) : base(client)
        {
            _chatRepository = chatRepository;
        }

        public override async Task Execute()
        {
            
        }
    }
}