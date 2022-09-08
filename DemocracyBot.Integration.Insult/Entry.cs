using System;
using Microsoft.Extensions.DependencyInjection;

namespace DemocracyBot.Integration.Insult
{
    public static class Entry
    {
        public static IServiceCollection AddInsultIntegration(this IServiceCollection services)
        {
            services.AddHttpClient<IEvilInsultService, EvilInsultService>();

            return services;
        }
    }
}