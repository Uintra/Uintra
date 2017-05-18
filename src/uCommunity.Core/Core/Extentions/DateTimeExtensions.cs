using System;
using System.Web;
using Compent.uCommunity.Core;

namespace uCommunity.Core.Extentions
{
    public static class DateTimeExtensions
    {
        public static string ToDefaultDateFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            return date.ToString(dateTimeFormatProvider.DefaultDateFormat);
        }

        public static string ToDefaultTimeFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            return date.ToString(dateTimeFormatProvider.DefaultTimeFormat);
        }

        public static string ToDefaultDateTimeFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            return date.ToString(dateTimeFormatProvider.DefaultDateTimeFormat);
        }

        public static string ToDefaultDatePickerFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            return date.ToString(dateTimeFormatProvider.DefaultDatePickerFormat);
        }

        public static string ToDefaultDateTimePickerFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            return date.ToString(dateTimeFormatProvider.DefaultDateTimePickerFormat);
        }

        public static string ToDefaultDateTimeValuePickerFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            return date.ToString(dateTimeFormatProvider.DefaultDateTimeValuePickerFormat);
        }
    }
}
