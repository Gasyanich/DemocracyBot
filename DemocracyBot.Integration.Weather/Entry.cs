using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DemocracyBot.Integration.Weather
{
    public static class Entry
    {
        public static IServiceCollection AddWeatherIntegration(this IServiceCollection services)
        {
            services.AddHttpClient<IWeatherService, WeatherService>();

            return services;
        }
    }
}