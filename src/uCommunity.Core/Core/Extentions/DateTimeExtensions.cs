using System;
using System.Web;
using Compent.uCommunity.Core;

namespace uCommunity.Core.Extentions
{
    public static class DateTimeExtensions
    {
        public static string ToDateFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            return date.ToString(dateTimeFormatProvider.DateFormat);
        }

        public static string ToTimeFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            return date.ToString(dateTimeFormatProvider.TimeFormat);
        }

        public static string ToDateTimeFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            return date.ToString(dateTimeFormatProvider.DateTimeFormat);
        }

        public static string ToDatePickerFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            return date.ToString(dateTimeFormatProvider.DatePickerFormat);
        }

        public static string ToDateTimePickerFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            return date.ToString(dateTimeFormatProvider.DateTimePickerFormat);
        }

        public static string ToDateTimeValuePickerFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            return date.ToString(dateTimeFormatProvider.DateTimeValuePickerFormat);
        }
    }
}
