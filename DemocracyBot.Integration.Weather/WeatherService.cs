using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DemocracyBot.Integration.Weather.Dto;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DemocracyBot.Integration.Weather
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly WeatherApiSettings _weatherApiSettings;

        public WeatherService(
            HttpClient httpClient,
            IOptions<WeatherApiSettings> options)
        {
            _httpClient = httpClient;
            _weatherApiSettings = options.Value;
        }

        public async Task<WeatherDto> GetWeather()
        {
            var queryParams = new Dictionary<string, string>
            {
                ["lat"] = _weatherApiSettings.Lat,
                ["lon"] = _weatherApiSettings.Lon,
                ["appid"] = _weatherApiSettings.AppId,
                ["units"] = _weatherApiSettings.Units,
                ["lang"] = _weatherApiSettings.Lang
            };

            var requestUri = QueryHelpers.AddQueryString(_weatherApiSettings.ApiUrl, queryParams);

            var weatherResponse = await _httpClient.GetAsync(requestUri);

            var weatherJson = await weatherResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<WeatherDto>(weatherJson);
        }
    }
}