using Microsoft.Extensions.DependencyInjection;

namespace DemocracyBot.Domain.Notification
{
    public static class Entry
    {
        public static IServiceCollection AddNotifications(this IServiceCollection services)
        {
            return services;
        }
    }
}