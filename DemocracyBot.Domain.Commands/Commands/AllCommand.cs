using System.Linq;
using System.Threading.Tasks;
using DemocracyBot.DataAccess.Repository.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Utils;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace DemocracyBot.Domain.Commands.Commands
{
    [Command("all")]
    public class AllCommand : CommandBase
    {
        private readonly IChatRepository _chatRepository;

        public AllCommand(TelegramBotClient client, IChatRepository chatRepository) : base(client)
        {
            _chatRepository = chatRepository;
        }

        public override async Task Execute()
        {
            var chatWithUsers = await _chatRepository.GetByChatId(ChatId);

            var mentionsText = chatWithUsers.Users.Select(MentionHelper.GetMentionByUser);

            var separatedMentions = string.Join(", ", mentionsText);

            var messageText = Message.Text?.Replace("/all", "");
            if (string.IsNullOrEmpty(messageText))
                return;

            var resultMessageToAll = separatedMentions + messageText;

            await Client.SendTextMessageAsync(ChatId, resultMessageToAll, ParseMode.Html);
        }
    }
}