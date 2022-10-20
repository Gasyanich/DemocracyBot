using System.Linq;
using System.Threading.Tasks;
using DemocracyBot.DataAccess.Entities;
using DemocracyBot.DataAccess.Repository;
using DemocracyBot.DataAccess.Repository.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions.CommandsBase;
using DemocracyBot.Domain.Commands.Utils;
using DemocracyBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace DemocracyBot.Domain.Commands.Commands
{
    [Command("join_meet")]
    public class JoinMeetCommand : CallbackQueryCommandBase
    {
        private readonly IMeetRepository _meetRepository;

        public JoinMeetCommand(TelegramBotClient client, IMeetRepository meetRepository) : base(client)
        {
            _meetRepository = meetRepository;
        }

        public override async Task Execute()
        {
            var meetId = long.Parse(CallbackQuery.Data!.Replace("/join_meet ", ""));

            var meet = await _meetRepository.GetMeetById(meetId);

            if(meet.Users.Any(u => u.Id == UserId))
                return;
            
            meet.Users.Add(new BotUser
            {
                Id = UserId
            });

            await _meetRepository.UpdateMeet(meet);

            var userName = CallbackQuery!.From.Username ?? CallbackQuery.From.FirstName;
            var message = $"{MentionHelper.GetMentionByUser(UserId, userName)} присоединяется к тусовке! Красавчик";

            await Client.SendTextMessageAsync(ChatId, message, ParseMode.Html);
        }
    }
}