using System;
using System.Threading.Tasks;
using DemocracyBot.DataAccess.Repository.Abstractions;
using DemocracyBot.Domain.Notification.Abstractions;
using DemocracyBot.Integration.Telegram.TgMessages;
using DemocracyBot.Integration.Weather;
using Telegram.Bot;

namespace DemocracyBot.Domain.Notification
{
    public class TimeOfDayJobService : ITimeOfDayJobService
    {
        private readonly IWeatherService _weatherService;
        private readonly IChatRepository _chatRepository;
        private readonly TelegramBotClient _client;

        public TimeOfDayJobService(IWeatherService weatherService,
            IChatRepository chatRepository,
            TelegramBotClient client)
        {
            _weatherService = weatherService;
            _chatRepository = chatRepository;
            _client = client;
        }

        public async Task GoodMorningJob()
        {
            var weather = await _weatherService.GetWeather();

            var temp = Convert.ToInt32(weather.main.temp);
            var feelsLike = Convert.ToInt32(weather.main.feels_like);

            var activeChats = await _chatRepository.GetActiveChats();

            foreach (var activeChat in activeChats)
            {
                var messages = TgMessageChain.Create(activeChat.Id)
                    .TextMessage("Всем доброе утро!" +
                                    $"\nПо данным портала openweathermap температура воздуха за окном {temp}℃, ощущается как {feelsLike}℃" +
                                    "\nОтличного дня!")
                    .StickerMessage("CAACAgIAAxkBAAEFptJjBOV_K3t0aQpBChb1ET7B4KpdPwACNRoAAns7MEr3TrYRo7Bt0ykE");

                await _client.Execute(messages);
            }
        }

        public Task GoodNightJob()
        {
            throw new System.NotImplementedException();
        }
    }
}