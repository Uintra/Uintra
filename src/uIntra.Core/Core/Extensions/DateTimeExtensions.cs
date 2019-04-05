using System;
using System.Linq;
using System.Web;
using Uintra.Core.ApplicationSettings;

namespace Uintra.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToIsoUtcString(this DateTime date)
        {
            return date.ToUniversalTime().ToString("o");
        }

        public static string ToDateFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            date = date.WithUserOffset();
            return date.ToString(dateTimeFormatProvider.DateFormat);
        }

        public static string ToTimeFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            date = date.WithUserOffset();
            return date.ToString(dateTimeFormatProvider.TimeFormat);
        }

        public static string ToEventDetailsDateFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            date = date.WithUserOffset();
            return date.ToString(dateTimeFormatProvider.EventDetailsDateFormat);
        }

        public static string ToEventDetailsTimeFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            date = date.WithUserOffset();
            return date.ToString(date.Minute > 0 ? dateTimeFormatProvider.EventDetailsTimeFormat : dateTimeFormatProvider.EventDetailsTimeWithoutMinutesFormat);
        }

        public static string ToDateTimeFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            date = date.WithUserOffset();
            return date.ToString(dateTimeFormatProvider.DateTimeFormat);
        }

        public static string ToDateTimeValuePickerFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            date = date.WithUserOffset();
            return date.ToString(dateTimeFormatProvider.DateTimeValuePickerFormat);
        }

        public static DateTime WithUserOffset(this DateTime date)
        {
            var timezoneOffsetProvider = HttpContext.Current.GetService<ITimezoneOffsetProvider>();
            var offset = timezoneOffsetProvider.GetTimezoneOffset();

            return date + offset;
        }

        public static DateTime WithCorrectedDaylightSavingTime(this DateTime utc, DateTime source)
        {
            var appSettings = HttpContext.Current.GetService<IApplicationSettings>();

            if (!appSettings.DaytimeSavingOffset && source.IsDaylightSavingTime())
            {
                var currentYear = DateTime.Now.Year;

                var corrected = TimeZoneInfo.Local.GetAdjustmentRules()
                    .Where(adjustmentRule => adjustmentRule.DateStart.Year <= currentYear && adjustmentRule.DateEnd.Year >= currentYear)
                    .Aggregate(utc, (current, adjustmentRule) => current.Add(adjustmentRule.DaylightDelta));
                return corrected;              
            }
            else
            {
                return utc;
            }
        }
    }
}
