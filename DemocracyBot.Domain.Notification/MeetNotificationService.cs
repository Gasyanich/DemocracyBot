using System.Linq;
using System.Threading.Tasks;
using DemocracyBot.DataAccess.Entities;
using DemocracyBot.DataAccess.Repository.Abstractions;
using DemocracyBot.Domain.Notification.Abstractions;
using DemocracyBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace DemocracyBot.Domain.Notification;

public class MeetNotificationService : IMeetNotificationService
{
    private readonly IMeetRepository _meetRepository;
    private readonly TelegramBotClient _botClient;

    public MeetNotificationService(IMeetRepository meetRepository, TelegramBotClient botClient)
    {
        _meetRepository = meetRepository;
        _botClient = botClient;
    }

    public async Task NotifyBeforeMeet(long meetId)
    {
        var meet = await _meetRepository.GetMeetById(meetId);
        var mentions = GetMeetMentions(meet);

        var message = mentions + $"надеюсь, вы не забыли про встречу с друзьями через час?" +
                      $"\nМесто встречи: {meet.Place}" +
                      $"\n\nОпаздывая, помните: пунктуальность - вежливость королей. А королей среди вас не наблюдаю";

        await _botClient.SendTextMessageAsync(meet.ChatId, message, ParseMode.Html);
    }

    public async Task NotifyMeetStart(long meetId)
    {
        var meet = await _meetRepository.GetMeetById(meetId);
        var mentions = GetMeetMentions(meet);

        var message = mentions + $"встреча началась, рой пидрил. Кто опоздал - тот пидорас"
            +$"\nМесто встречи: {meet.Place}";
        
        await _botClient.SendTextMessageAsync(meet.ChatId, message, ParseMode.Html);
    }

    private static string GetMeetMentions(Meet meet)
    {
        var mentionsText = meet.Users.Select(u => MentionHelper.GetMentionByUser(u.Id, u.Username));
        return string.Join(", ", mentionsText) + " ";
    }
}