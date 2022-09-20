using System.Threading.Tasks;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions.CommandsBase;
using DemocracyBot.Domain.Commands.Utils;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace DemocracyBot.Domain.Commands.Commands
{
    [Command("miss_meet")]
    public class MissMeetCommand : MessageCommandBase
    {
        public MissMeetCommand(TelegramBotClient client) : base(client)
        {
        }

        public override async Task Execute()
        {
            var userName = CallbackQuery!.From.Username ?? CallbackQuery.From.FirstName;

            var message = $"{MentionHelper.GetMentionByUser(UserId, userName)}, кто пас - тот пидорас. ";

            await Client.SendTextMessageAsync(ChatId, message, ParseMode.Html);
        }
    }
}