using System;
using System.Threading.Tasks;
using DemocracyBot.DataAccess.Repository.Abstractions;
using DemocracyBot.Domain.Notification.Abstractions;
using DemocracyBot.Domain.Notification.Dto;
using DemocracyBot.Integration.Weather;

namespace DemocracyBot.Domain.Notification
{
    public class TimeOfDayJobService : ITimeOfDayJobService
    {
        private readonly IWeatherService _weatherService;
        private readonly INotificationService _notificationService;
        private readonly IChatRepository _chatRepository;

        public TimeOfDayJobService(IWeatherService weatherService,
            INotificationService notificationService,
            IChatRepository chatRepository)
        {
            _weatherService = weatherService;
            _notificationService = notificationService;
            _chatRepository = chatRepository;
        }

        public async Task GoodMorningJob()
        {
            var weather = await _weatherService.GetWeather();

            var temp = Convert.ToInt32(weather.main.temp);
            var feelsLike = Convert.ToInt32(weather.main.feels_like);

            var goodMorningMessage = "Всем доброе утро!" +
                                     $"\nПо данным портала openweathermap температура воздуха за окном {temp}℃, ощущается как {feelsLike}℃" +
                                     "\nОтличного дня!";

            var activeChats = await _chatRepository.GetActiveChats();

            foreach (var activeChat in activeChats)
            {
                var notificationMessageDto = new NotificationMessageDto
                {
                    ChatId = activeChat.Id,
                    MessageText = goodMorningMessage,
                    AfterMessageStickerFileId = "CAACAgIAAxkBAAEFptJjBOV_K3t0aQpBChb1ET7B4KpdPwACNRoAAns7MEr3TrYRo7Bt0ykE"
                };

                await _notificationService.SendNotificationToChat(notificationMessageDto);
            }
        }

        public Task GoodNightJob()
        {
            throw new System.NotImplementedException();
        }
    }
}