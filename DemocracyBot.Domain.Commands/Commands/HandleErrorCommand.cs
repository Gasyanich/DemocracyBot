using System.Threading.Tasks;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions.Interactive;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace DemocracyBot.Domain.Commands.Commands
{
    [Command("handle_error")]
    public class HandleErrorCommand : CommandBase
    {
        private readonly IStateManager _stateManager;

        public HandleErrorCommand(TelegramBotClient client, IStateManager stateManager) : base(client)
        {
            _stateManager = stateManager;
        }

        public override async Task Execute()
        {
            _stateManager.RemoveState(UserId);

            await Reply("Вахуи пон. Ошибка", new ReplyKeyboardRemove());
        }
    }
}