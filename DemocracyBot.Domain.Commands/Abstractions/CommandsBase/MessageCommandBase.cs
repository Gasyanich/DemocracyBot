using Telegram.Bot;

namespace DemocracyBot.Domain.Commands.Abstractions.CommandsBase
{
    public abstract class MessageCommandBase : CommandBase
    {
        protected MessageCommandBase(TelegramBotClient client) : base(client)
        {
        }
        
        protected override long ChatId => Message.Chat.Id;
        protected override long UserId => Message.From!.Id;
    }
}