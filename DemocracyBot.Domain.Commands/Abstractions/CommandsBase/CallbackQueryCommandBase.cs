using Telegram.Bot;

namespace DemocracyBot.Domain.Commands.Abstractions.CommandsBase
{
    public abstract class CallbackQueryCommandBase : CommandBase
    {
        protected CallbackQueryCommandBase(TelegramBotClient client) : base(client)
        {
        }

        protected override long ChatId => CallbackQuery.Message!.Chat.Id;
        protected override long UserId => CallbackQuery.From.Id;
    }
}