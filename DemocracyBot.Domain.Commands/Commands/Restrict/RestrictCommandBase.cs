using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DemocracyBot.DataAccess.Repository.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions.Interactive;
using Hangfire;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace DemocracyBot.Domain.Commands.Commands.Restrict;

public abstract class RestrictCommandBase : InteractiveCommandBase<RestrictState, RestrictStep>
{
    private readonly IChatRepository _chatRepository;
    private readonly IRestrictService _restrictService;

    protected RestrictCommandBase(TelegramBotClient client,
        IStateManager stateManager,
        IChatRepository chatRepository,
        IRestrictService restrictService)
        : base(client, stateManager)
    {
        _chatRepository = chatRepository;
        _restrictService = restrictService;
    }

    private static IReplyMarkup AnswerUsernameMarkup => GetForceReplyMarkup("Введи логин преступника");

    protected override async Task<RestrictStep> HandleStart()
    {
        await Reply("Кого отправляем в карцер? Тэгни преступника через '@'. Например, @gasyanich",
            AnswerUsernameMarkup);

        return RestrictStep.AnswerUsername;
    }

    protected override async Task<RestrictStep> HandleStep(RestrictStep step)
    {
        if (State.CurrentStep != RestrictStep.AnswerUsername) return default;

        var userName = Message.Text?.Replace("@", "").Trim();

        if (userName == null)
        {
            await Reply("У нас в базе таких нет.. Проверь, что тэгнул правильно", AnswerUsernameMarkup);
            return RestrictStep.AnswerUsername;
        }

        var user = await _chatRepository.GetChatUserByUserName(ChatId, userName);

        if (user == null)
        {
            await Reply("У нас в базе таких нет.. Проверь, что тэгнул правильно", AnswerUsernameMarkup);
            return RestrictStep.AnswerUsername;
        }

        var chatUserData = await Client.GetChatMemberAsync(ChatId, user.Id);

        if (chatUserData.Status == ChatMemberStatus.Creator)
        {
            await Reply("Правителя нельзя наказать, даже если очень хочется. Терпим..");
            return default;
        }

        var pollAnswers = new List<string> {"Да, пускай подумает над поведением", "No."};
        const int pollDurationMin = 5;
        var pollQuestion = $"Отправим @{user.Username} подумать над поведением в карцер?" +
                           $" Время на подумать - {pollDurationMin}мин, а то вдруг эта сука котят взрывает, минимизируем жертвы";

        var message = await Client.SendPollAsync(
            ChatId,
            pollQuestion,
            pollAnswers,
            false,
            allowsMultipleAnswers: false
        );

        await Client.PinChatMessageAsync(ChatId, message.MessageId, true);

        BackgroundJob.Schedule<IRestrictService>(
            service => service.RestrictUserByPoll(message.MessageId, ChatId, user),
            TimeSpan.FromMinutes(pollDurationMin)
        );

        return default;
    }
}