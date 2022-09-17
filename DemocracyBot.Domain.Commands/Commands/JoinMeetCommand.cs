using System.Threading.Tasks;
using DemocracyBot.DataAccess.Repository.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Utils;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace DemocracyBot.Domain.Commands.Commands
{
    [Command("join_meet")]
    public class JoinMeetCommand : CommandBase
    {
        public JoinMeetCommand(TelegramBotClient client) : base(client)
        {
        }

        public override async Task Execute()
        {
            var userName = Update.CallbackQuery!.From.Username ?? Update.CallbackQuery.From.FirstName;

            var message = $"{MentionHelper.GetMentionByUser(Update.CallbackQuery.From.Id, userName)} присоединяется к тусовке! Красавчик";

            await Client.SendTextMessageAsync(ChatId, message, ParseMode.Html);
        }
    }
}