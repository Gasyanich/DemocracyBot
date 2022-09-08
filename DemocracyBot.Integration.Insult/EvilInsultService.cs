using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DemocracyBot.Integration.Insult
{
    public class EvilInsultService : IEvilInsultService
    {
        private readonly HttpClient _httpClient;
        private readonly EvilInsultApiSettings _insultApiSettings;

        public EvilInsultService(HttpClient httpClient, IOptions<EvilInsultApiSettings> options)
        {
            _httpClient = httpClient;
            _insultApiSettings = options.Value;
        }

        public async Task<string> GetInsult()
        {
            var apiUrl = _insultApiSettings.ApiUrl;

            var dateTimeNowString = DateTime.Now;
            
            var insultResponse = await _httpClient.GetAsync(apiUrl + $"/generate_insult.php?lang=ru&type=json&rnd={dateTimeNowString}");

            var insultJson = await insultResponse.Content.ReadAsStringAsync();

            var insult = JsonConvert.DeserializeObject<InsultResponse>(insultJson);

            return insult?.Insult;
        }
    }
}