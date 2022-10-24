using DemocracyBot.DataAccess.Repository.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions.Interactive;
using Telegram.Bot;

namespace DemocracyBot.Domain.Commands.Commands.Restrict;

[Command("voteban")]
public class BanCommand : RestrictCommandBase
{
    public BanCommand(TelegramBotClient client,
        IStateManager stateManager,
        IChatRepository chatRepository,
        IRestrictService restrictService)
        : base(client, stateManager, chatRepository, restrictService)
    {
    }
}