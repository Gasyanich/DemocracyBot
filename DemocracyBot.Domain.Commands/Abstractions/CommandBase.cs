using System.Threading.Tasks;
using DemocracyBot.Domain.Commands.Commands.Common;
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

        public CommandType Type => CommandType.Common;

        protected long ChatId => Message.Chat.Id;
        
        protected long UserId => Message.From!.Id;

        protected async Task SendTextMessage(string text)
        {
            await Client.SendTextMessageAsync(ChatId, text);
        }

        #region Utils

        protected virtual async Task<Message> Reply(string messageText, IReplyMarkup replyMarkup = null)
        {
            var message = await Client.SendTextMessageAsync(
                ChatId,
                messageText,
                replyToMessageId:Message.MessageId,
                replyMarkup:replyMarkup
            );

            return message;
        }

        #endregion
    }
}