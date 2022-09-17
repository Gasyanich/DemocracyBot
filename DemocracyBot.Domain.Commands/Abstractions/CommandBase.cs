using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

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

        #region Utils

        protected static IReplyMarkup GetForceReplyMarkup(string text)
        {
            return new ForceReplyMarkup
            {
                Selective = true,
                InputFieldPlaceholder = text
            };
        }
        
        protected async Task SendTextMessage(string text)
        {
            await Client.SendTextMessageAsync(ChatId, text);
        }

        protected virtual async Task<Message> Reply(string messageText, IReplyMarkup replyMarkup = null)
        {
            var message = await Client.SendTextMessageAsync(
                ChatId,
                messageText,
                replyToMessageId: Message.MessageId,
                replyMarkup: replyMarkup
            );

            return message;
        }

        protected long ChatId => Message.Chat.Id;

        protected long UserId => Message.From!.Id;

        #endregion
    }
}