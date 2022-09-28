using System;
using DemocracyBot.Domain.Notification.Abstractions;
using Hangfire;
using Microsoft.Extensions.Configuration;

namespace DemocracyBot.Web.Hangfire
{
    public static class HangfireJobRegisterHelper
    {
        public static void RegisterJobs(IConfiguration configuration)
        {
            var settings = configuration.GetSection("DayOfTimeJobSettings").Get<DayOfTimeJobSettings>();

            var dayNotificationHour = settings.DayNotification - settings.Offset;
            var nightNotificationHour = settings.NightNotification - settings.Offset;

            RecurringJob.AddOrUpdate<ITimeOfDayJobService>(
                service => service.GoodMorningJob(),
                Cron.Daily(dayNotificationHour),
                TimeZoneInfo.Utc
            );
            
            RecurringJob.AddOrUpdate<IBirthDayNotificationService>(
                service => service.HappyBirthDay(),
                Cron.Daily(20),
                TimeZoneInfo.Utc
                );
        }
    }
}