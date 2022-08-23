namespace DemocracyBot.Integration.Weather.Dto
{
    public class WeatherApiSettings
    {
        /// <summary>
        /// API url openweather
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// Широта
        /// </summary>
        public string Lat { get; set; }

        /// <summary>
        /// Долгота
        /// </summary>
        public string Lon { get; set; }

        /// <summary>
        /// API key
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// Система счисления (метричная)
        /// </summary>
        public string Units { get; set; }

        /// <summary>
        /// Язык (ru)
        /// </summary>
        public string Lang { get; set; }
    }
}