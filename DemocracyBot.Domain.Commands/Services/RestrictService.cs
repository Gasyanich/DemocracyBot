using System;
using System.Threading.Tasks;
using DemocracyBot.DataAccess.Entities;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Integration.Telegram.Extensions;
using DemocracyBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DemocracyBot.Domain.Commands.Services;

public class RestrictService : IRestrictService
{
    private readonly TelegramBotClient _client;

    public RestrictService(TelegramBotClient client)
    {
        _client = client;
    }

    public async Task RestrictUserByPoll(int pollMessageId, long chatId, BotUser user)
    {
        var poll = await _client.StopPollAsync(chatId, pollMessageId);

        if (poll.TotalVoterCount < 2)
        {
            await _client.SendTextMessageAsync(
                chatId,
                $"Не набралось необхидмое количество голосов для вынесения приговора @{user.Username}. Нужно хотя бы 3"
            );

            return;
        }

        var yesVotesCount = poll.Options[0].VoterCount;
        var noVotesCount = poll.Options[1].VoterCount;

        var mention = MentionHelper.GetMentionByUser(user.Id, user.Username);

        if (yesVotesCount > noVotesCount)
        {
            var diffVotesCount = yesVotesCount - noVotesCount;

            var restrictDurationMin = diffVotesCount * 5;

            var chatMember = await _client.GetChatMemberAsync(chatId, user.Id);

            await _client.MuteUser(chatId, user.Id, restrictDurationMin, chatMember.Status);

            await _client.SendTextMessageAsync(
                chatId,
                $"{mention} получает по ебалу народным молотом правосудия. Эффект стана продлится {restrictDurationMin}мин",
                ParseMode.Html,
                replyToMessageId: pollMessageId);
        }
        else
        {
            await _client.SendTextMessageAsync(
                chatId,
                $"В этот раз {mention} избежал наказания",
                ParseMode.Html,
                replyToMessageId: pollMessageId);
        }
    }
}