using DemocracyBot.Domain.Notification.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DemocracyBot.Domain.Notification
{
    public static class Entry
    {
        public static IServiceCollection AddNotifications(this IServiceCollection services)
        {
            services.AddScoped<ITimeOfDayJobService, TimeOfDayJobService>();
            services.AddScoped<IBirthDayNotificationService, BirthDayNotificationService>();
            services.AddScoped<IMeetNotificationService, MeetNotificationService>();
            
            return services;
        }
    }
}