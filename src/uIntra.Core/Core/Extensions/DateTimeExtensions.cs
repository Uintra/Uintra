using System;
using System.Web;

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
            date = date.AddUserOffset();
            return date.ToString(dateTimeFormatProvider.DateFormat);
        }

        public static string ToTimeFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            date = date.AddUserOffset();
            return date.ToString(dateTimeFormatProvider.TimeFormat);
        }

        public static string ToEventDetailsDateFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            date = date.AddUserOffset();
            return date.ToString(dateTimeFormatProvider.EventDetailsDateFormat);
        }

        public static string ToEventDetailsTimeFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            date = date.AddUserOffset();
            return date.ToString(date.Minute > 0 ? dateTimeFormatProvider.EventDetailsTimeFormat : dateTimeFormatProvider.EventDetailsTimeWithoutMinutesFormat);
        }

        public static string ToDateTimeFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            date = date.AddUserOffset();
            return date.ToString(dateTimeFormatProvider.DateTimeFormat);
        }

        public static string ToDatePickerFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            date = date.AddUserOffset();
            return date.ToString(dateTimeFormatProvider.DatePickerFormat);
        }

        public static string ToDateTimePickerFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            date = date.AddUserOffset();
            return date.ToString(dateTimeFormatProvider.DateTimePickerFormat);
        }

        public static string ToDateTimeValuePickerFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            date = date.AddUserOffset();
            return date.ToString(dateTimeFormatProvider.DateTimeValuePickerFormat);
        }

        public static DateTime AddUserOffset(this DateTime date)
        {
            var timezoneOffsetProvider = HttpContext.Current.GetService<ITimezoneOffsetProvider>();
            var offset = timezoneOffsetProvider.GetTimezoneOffset();

            date = date.AddMinutes(offset);

            return date;
        }
    }
}
