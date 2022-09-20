using System.Threading.Tasks;
using DemocracyBot.DataAccess.Repository.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions.CommandsBase;
using DemocracyBot.Domain.Commands.Utils;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace DemocracyBot.Domain.Commands.Commands
{
    [Command("join_meet")]
    public class JoinMeetCommand : CallbackQueryCommandBase
    {
        public JoinMeetCommand(TelegramBotClient client) : base(client)
        {
        }

        public override async Task Execute()
        {
            var userName = CallbackQuery!.From.Username ?? CallbackQuery.From.FirstName;

            var message = $"{MentionHelper.GetMentionByUser(UserId, userName)} присоединяется к тусовке! Красавчик";

            await Client.SendTextMessageAsync(ChatId, message, ParseMode.Html);
        }
    }
}