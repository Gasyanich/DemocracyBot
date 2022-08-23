using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DemocracyBot.Domain.Commands.Abstractions
{
    public abstract class CommandBase : ICommand
    {
        protected Message Message;
        protected readonly TelegramBotClient Client;

        protected CommandBase(TelegramBotClient client)
        {
            Client = client;
        }

        public void Init(Message message)
        {
            Message = message;
        }

        public abstract Task Execute();

        protected long ChatId => Message.Chat.Id;

        protected async Task SendTextMessage(string text)
        {
            await Client.SendTextMessageAsync(ChatId, text);
        }
    }
}