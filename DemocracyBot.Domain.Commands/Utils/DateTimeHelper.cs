using System;
using System.Collections.Generic;
using System.Globalization;

namespace DemocracyBot.Domain.Commands.Utils
{
    public static class DateTimeHelper
    {
        private const string Today = "сегодня";
        private const string Tomorrow = "завтра";
        private const string DayAfter = "послезавтра";

        private static readonly List<DayOfTheWeek> DayOfTheWeeks = new List<DayOfTheWeek>
        {
            new DayOfTheWeek("воскресенье", "вс", DayOfWeek.Sunday),
            new DayOfTheWeek("понедельник", "пн", DayOfWeek.Monday),
            new DayOfTheWeek("вторник", "вт", DayOfWeek.Tuesday),
            new DayOfTheWeek("среда", "ср", DayOfWeek.Wednesday),
            new DayOfTheWeek("четверг", "чт", DayOfWeek.Thursday),
            new DayOfTheWeek("пятница", "пт", DayOfWeek.Friday),
            new DayOfTheWeek("суббота", "сб", DayOfWeek.Saturday),
        };

        public static DateTimeOffset? ParseDateFromString(string dateStr, int timeZoneOffset)
        {
            var dateStrLower = dateStr.ToLower().Split(' ')[0];

            var time = dateStr.Split(' ')[1];

            var dateTime = GetDateFromRelative(dateStrLower)
                           ?? GetDateFromDayOfTheeWeek(dateStrLower)
                           ?? GetDateFromDateStr(dateStrLower);

            if (dateTime != null)
            {
                var timeValues = time.Split(':');

                var hours = int.Parse(timeValues[0]);
                var minutes = int.Parse(timeValues[1]);

                var date = dateTime.Value;

                var meetDate = new DateTime(date.Year, date.Month, date.Day, hours, minutes, 0);

                return new DateTimeOffset(meetDate, TimeSpan.FromHours(timeZoneOffset));
            }

            return null;
        }


        /// <summary>
        /// Получить дату по строке относительно текущего дня - сегодня/завтра/послезавтра
        /// </summary>
        private static DateTime? GetDateFromRelative(string dateStrLower) => dateStrLower switch
        {
            Today => DateTime.Now,
            Tomorrow => DateTime.Now.AddDays(1),
            DayAfter => DateTime.Now.AddDays(2),
            _ => null
        };

        /// <summary>
        /// Получить дату по названию дня недели
        /// </summary>
        private static DateTime? GetDateFromDayOfTheeWeek(string dateStrLower)
        {
            var dayOfWeek = GetDayOfWeek(dateStrLower);

            if (dayOfWeek == null) return null;

            var currentDate = DateTime.Now;
            while (currentDate.DayOfWeek != dayOfWeek)
                currentDate = currentDate.AddDays(1);

            return currentDate;
        }

        /// <summary>
        /// Получить дату по строке формата dd.MM
        /// </summary>
        private static DateTime? GetDateFromDateStr(string dateStrLower)
        {
            var fullDate = dateStrLower + $".{DateTime.Now.Year}";

            if (DateTime.TryParseExact(fullDate, "dd.MM.yyyy", null, DateTimeStyles.None, out var dateTime))
                return dateTime;

            return null;
        }

        private static DayOfWeek? GetDayOfWeek(string dateStrLower)
        {
            foreach (var dayOfTheWeek in DayOfTheWeeks)
            {
                if (dayOfTheWeek.TryGetDayOfWeek(dateStrLower, out var dayOfWeek))
                    return dayOfWeek;
            }

            return null;
        }


        private class DayOfTheWeek
        {
            public DayOfTheWeek(string dayName, string dayNameShort, DayOfWeek day)
            {
                DayName = dayName;
                DayNameShort = dayNameShort;
                Day = day;
            }

            private string DayName { get; }

            private string DayNameShort { get; }

            private DayOfWeek Day { get; }

            public bool TryGetDayOfWeek(string dateStrLowerCase, out DayOfWeek? day)
            {
                day = null;

                if (DayName.ToLower() != dateStrLowerCase && DayNameShort.ToLower() != dateStrLowerCase)
                    return false;

                day = Day;
                return true;
            }
        }
    }
}