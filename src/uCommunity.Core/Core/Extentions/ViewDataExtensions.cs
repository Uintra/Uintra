using System.Web;
using System.Web.Mvc;
using Compent.uCommunity.Core;

namespace uCommunity.Core.Extentions
{
    public static class ViewDataExtensions
    {
        private const string DateFormatAlias = "DateFormat";
        private const string TimeFormatAlias = "TimeFormat";
        private const string DateTimeFormatAlias = "DateTimeFormat";
        private const string DateTimeValuePickerFormatAlias = "DateTimeValuePickerFormat";
        private const string DatePickerFormatAlias = "DatePickerFormat";
        private const string DateTimePickerFormatAlias = "DateTimePickerFormat";
        

        public static void SetDateTimeFormats(this ViewDataDictionary viewData)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            viewData[DateFormatAlias] = dateTimeFormatProvider.DateFormat;
            viewData[TimeFormatAlias] = dateTimeFormatProvider.TimeFormat;
            viewData[DateTimeFormatAlias] = dateTimeFormatProvider.DateTimeFormat;
            viewData[DateTimeValuePickerFormatAlias] = dateTimeFormatProvider.DateTimeValuePickerFormat;
            viewData[DatePickerFormatAlias] = dateTimeFormatProvider.DatePickerFormat;
            viewData[DateTimePickerFormatAlias] = dateTimeFormatProvider.DateTimePickerFormat;
        }

        public static string GetDateFormat(this ViewDataDictionary viewData)
        {
            return viewData[DateFormatAlias]?.ToString();
        }

        public static string GetTimeFormat(this ViewDataDictionary viewData)
        {
            return viewData[TimeFormatAlias]?.ToString();
        }

        public static string GetDateTimeFormat(this ViewDataDictionary viewData)
        {
            return viewData[DateTimeFormatAlias]?.ToString();
        }

        public static string GetDatePickerFormat(this ViewDataDictionary viewData)
        {
            return viewData[DatePickerFormatAlias]?.ToString();
        }

        public static string GetDateTimePickerFormat(this ViewDataDictionary viewData)
        {
            return viewData[DateTimePickerFormatAlias]?.ToString();
        }

        public static string GetDateTimeValueFormat(this ViewDataDictionary viewData)
        {
            return viewData[DateTimeValuePickerFormatAlias]?.ToString();
        }
    }
}
