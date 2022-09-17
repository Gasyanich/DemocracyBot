using System;

namespace DemocracyBot.Domain.Commands.Utils
{
    public static class DayOfTheWeekHelper
    {
        public static string GetDayOfTheWeekTextByDate(DateTimeOffset dateTime) => dateTime.DayOfWeek switch
        {
            DayOfWeek.Sunday => "Воскресенье",
            DayOfWeek.Monday => "Понедельник",
            DayOfWeek.Tuesday => "Вторник",
            DayOfWeek.Wednesday => "Среда",
            DayOfWeek.Thursday => "Четверг",
            DayOfWeek.Friday => "Пятница",
            DayOfWeek.Saturday => "Суббота",
        };
    }
}