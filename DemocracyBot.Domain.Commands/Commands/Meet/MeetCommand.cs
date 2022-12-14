using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DemocracyBot.DataAccess.Repository.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions;
using DemocracyBot.Domain.Commands.Abstractions.Interactive;
using DemocracyBot.Domain.Commands.Utils;
using DemocracyBot.Domain.Notification.Abstractions;
using Hangfire;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace DemocracyBot.Domain.Commands.Commands.Meet
{
    [Command("meet")]
    public class MeetCommand : InteractiveCommandBase<MeetState, MeetStep>
    {
        private readonly IMeetRepository _meetRepository;

        public MeetCommand(TelegramBotClient client,
            IStateManager stateManager,
            IMeetRepository meetRepository)
            : base(client, stateManager)
        {
            _meetRepository = meetRepository;
        }

        protected override async Task<MeetStep> HandleStart()
        {
            await Reply("Где планируется сбор?", AskingForPlaceMarkup);

            return MeetStep.AskingForPlace;
        }

        protected override async Task<MeetStep> HandleStep(MeetStep step)
        {
            return step switch
            {
                MeetStep.AskingForPlace => await HandleAskingForPlace(),
                MeetStep.AskingForDateTime => await HandleAskingForDateTime(),
                _ => default
            };
        }

        private static IReplyMarkup AskingForPlaceMarkup => GetForceReplyMarkup("Укажи предполагаемое место встречи");

        private static IReplyMarkup AskingForDateTimeMarkup => GetForceReplyMarkup("Укажи дату встречи");

        private async Task<MeetStep> HandleAskingForPlace()
        {
            var place = Message.Text?.Trim();
            if (string.IsNullOrWhiteSpace(place))
            {
                await Reply("Не понял нихуя. Где сбор то?", AskingForPlaceMarkup);
                return MeetStep.AskingForPlace;
            }

            State.MeetPlace = place;

            await Reply(
                "Окей, теперь определимся со временем - когда планируется сбор?" +
                "\n\nПримеры для глупых:" +
                "\nСегодня 21:30" +
                "\nПонедельник 18:15" +
                "\nпт 22:00" +
                "\n21.06 16:45",
                AskingForDateTimeMarkup
            );

            return MeetStep.AskingForDateTime;
        }

        private async Task<MeetStep> HandleAskingForDateTime()
        {
            var dateTimeText = Message.Text?.Trim();
            if (string.IsNullOrWhiteSpace(dateTimeText))
            {
                await Reply(
                    "Что-то пошло не так. Проверь, что дата и время в допустимом формате",
                    AskingForDateTimeMarkup
                );

                return MeetStep.AskingForDateTime;
            }

            try
            {
                var meetDate = DateTimeHelper.ParseDateFromString(dateTimeText, 4);

                if (meetDate != null)
                {
                    // if (meetDate < DateTimeOffset.Now)
                    // {
                    //     await Reply(
                    //         "Охуеть ты придумал, давай в прошлом встретимся, я же уже машину времени изобрел. Пробуй еще",
                    //         AskingForDateTimeMarkup
                    //     );
                    //
                    //     return MeetStep.AskingForDateTime;
                    // }
                    //
                    // if (meetDate - DateTimeOffset.Now < TimeSpan.FromHours(1))
                    // {
                    //     await Reply(
                    //         "Давай попробуем планировать встречи хотя бы за час?)) пробуй еще",
                    //         AskingForDateTimeMarkup
                    //     );
                    //
                    //     return MeetStep.AskingForDateTime;
                    // }

                    
                    State.MeetDateTime = meetDate.Value;

                    var meet = new DataAccess.Entities.Meet()
                    {
                        ChatId = ChatId,
                        Date = State.MeetDateTime,
                        Place = State.MeetPlace
                    };

                    await _meetRepository.CreateMeet(meet);

                    var inlineKeyboard = new InlineKeyboardMarkup(new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData("Я в деле", $"/join_meet {meet.Id}"),
                        InlineKeyboardButton.WithCallbackData("Я пас", $"/miss_meet {meet.Id}"),
                    });

                    var meetMessage = await Reply(
                        "Готово!\n" +
                        $"\nМесто встречи: {State.MeetPlace}" +
                        $"\nДата и время встречи: {DayOfTheWeekHelper.GetDayOfTheWeekTextByDate(State.MeetDateTime)} {State.MeetDateTime:dd.MM, HH:mm}",
                        inlineKeyboard);

                    await Client.PinChatMessageAsync(ChatId, meetMessage.MessageId, false);

                    var beforeMeetNotifyTime = meet.Date.AddHours(-1);

                    BackgroundJob.Schedule<IMeetNotificationService>(service => service.NotifyBeforeMeet(meet.Id),
                        beforeMeetNotifyTime);
                    BackgroundJob.Schedule<IMeetNotificationService>(service => service.NotifyMeetStart(meet.Id),
                        meet.Date);

                    return default;
                }
            }
            catch (Exception e)
            {
                await Reply(
                    "Что-то пошло не так. Проверь, что дата и время в допустимом формате",
                    AskingForDateTimeMarkup
                );

                return MeetStep.AskingForDateTime;
            }

            return MeetStep.AskingForDateTime;
        }
    }
}