using System.Threading.Tasks;
using DemocracyBot.Integration.Weather.Dto;

namespace DemocracyBot.Integration.Weather
{
    public interface IWeatherService
    {
        Task<WeatherDto> GetWeather();
    }
}