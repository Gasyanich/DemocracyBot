using System.Linq;
using System.Threading.Tasks;
using DemocracyBot.DataAccess.Repository.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions.CommandsBase;
using DemocracyBot.Domain.Commands.Utils;
using DemocracyBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace DemocracyBot.Domain.Commands.Commands
{
    [Command("miss_meet")]
    public class MissMeetCommand : CallbackQueryCommandBase
    {
        private readonly IMeetRepository _meetRepository;

        public MissMeetCommand(TelegramBotClient client, IMeetRepository meetRepository) : base(client)
        {
            _meetRepository = meetRepository;
        }

        public override async Task Execute()
        {
            var meetId = long.Parse(CallbackQuery.Data!.Replace("/miss_meet ", ""));

            var meet = await _meetRepository.GetMeetById(meetId);

            var meetUser = meet.Users.FirstOrDefault(u => u.Id == UserId);

            if (meetUser != null)
            {
                meet.Users.Remove(meetUser);

                await _meetRepository.UpdateMeet(meet);
            }

            var userName = CallbackQuery!.From.Username ?? CallbackQuery.From.FirstName;
            var message = $"{MentionHelper.GetMentionByUser(UserId, userName)}, кто пас - тот пидорас. ";

            await Client.SendTextMessageAsync(ChatId, message, ParseMode.Html);
        }
    }
}