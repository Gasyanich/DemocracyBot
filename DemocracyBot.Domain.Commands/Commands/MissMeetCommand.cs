using System.Threading.Tasks;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Utils;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace DemocracyBot.Domain.Commands.Commands
{
    [Command("miss_meet")]
    public class MissMeetCommand : CommandBase
    {
        public MissMeetCommand(TelegramBotClient client) : base(client)
        {
        }

        public override async Task Execute()
        {
            var userName = Update.CallbackQuery!.From.Username ?? Update.CallbackQuery.From.FirstName;

            var message = $"{MentionHelper.GetMentionByUser(Update.CallbackQuery.From.Id, userName)}, кто пас - тот пидорас. ";

            await Client.SendTextMessageAsync(ChatId, message, ParseMode.Html);
        }
    }
}