using System.Linq;
using System.Threading.Tasks;
using DemocracyBot.DataAccess.Repository.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions.Interactive;
using DemocracyBot.Domain.Commands.Utils;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace DemocracyBot.Domain.Commands.Commands.All
{
    [Command("all")]
    public class AllCommand : InteractiveCommandBase<AllState, AllStep>
    {
        private readonly IChatRepository _chatRepository;

        public AllCommand(TelegramBotClient client,
            IStateManager stateManager,
            IChatRepository chatRepository) : base(client, stateManager)
        {
            _chatRepository = chatRepository;
        }

        protected override async Task<AllStep> HandleStart()
        {
            await Reply("Что передать остальным?", AskMessageForAllMarkup);

            return AllStep.AskMessageForAll;
        }

        protected override async Task<AllStep> HandleStep(AllStep step)
        {
            if (step != AllStep.AskMessageForAll) return default;

            var messageText = Message.Text?.Trim();
            if (string.IsNullOrWhiteSpace(messageText))
                return default;

            var chatWithUsers = await _chatRepository.GetByChatId(ChatId);

            var mentionsText = chatWithUsers.Users.Select(MentionHelper.GetMentionByUser);
            var separatedMentions = string.Join(", ", mentionsText);

            var resultMessageToAll = separatedMentions + " " + messageText;

            await Client.SendTextMessageAsync(ChatId, resultMessageToAll, ParseMode.Html);

            return default;
        }

        private static IReplyMarkup AskMessageForAllMarkup => new ForceReplyMarkup
        {
            Selective = true,
            InputFieldPlaceholder = "Введи сообщение для остальных"
        };
    }
}