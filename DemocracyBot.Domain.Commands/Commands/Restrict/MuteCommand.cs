using DemocracyBot.DataAccess.Repository.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions.Interactive;
using Telegram.Bot;

namespace DemocracyBot.Domain.Commands.Commands.Restrict;

[Command("votemute")]
public class MuteCommand : RestrictCommandBase
{
    public MuteCommand(TelegramBotClient client,
        IStateManager stateManager,
        IChatRepository chatRepository,
        IRestrictService restrictService)
        : base(client, stateManager, chatRepository, restrictService)
    {
    }
}