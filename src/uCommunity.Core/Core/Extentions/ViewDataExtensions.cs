using System.Web;
using System.Web.Mvc;
using Compent.uCommunity.Core;

namespace uCommunity.Core.Extentions
{
    public static class ViewDataExtensions
    {
        private const string DefaultDateFormatAlias = "DefaultDateFormat";
        private const string DefaultTimeFormatAlias = "DefaultTimeFormat";
        private const string DefaultDateTimeFormatAlias = "DefaultDateTimeFormat";
        private const string DefaultDateTimeValuePickerFormatAlias = "DefaultDateTimeValuePickerFormat";
        private const string DefaultDatePickerFormatAlias = "DefaultDatePickerFormat";
        private const string DefaultDateTimePickerFormatAlias = "DefaultDateTimePickerFormat";
        

        public static void SetDateTimeFormats(this ViewDataDictionary viewData)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            viewData[DefaultDateFormatAlias] = dateTimeFormatProvider.DefaultDateFormat;
            viewData[DefaultTimeFormatAlias] = dateTimeFormatProvider.DefaultTimeFormat;
            viewData[DefaultDateTimeFormatAlias] = dateTimeFormatProvider.DefaultDateTimeFormat;
            viewData[DefaultDateTimeValuePickerFormatAlias] = dateTimeFormatProvider.DefaultDateTimeValuePickerFormat;
            viewData[DefaultDatePickerFormatAlias] = dateTimeFormatProvider.DefaultDatePickerFormat;
            viewData[DefaultDateTimePickerFormatAlias] = dateTimeFormatProvider.DefaultDateTimePickerFormat;
        }

        public static string GetDefaultDateFormat(this ViewDataDictionary viewData)
        {
            return viewData[DefaultDateFormatAlias]?.ToString();
        }

        public static string GetDefaultTimeFormat(this ViewDataDictionary viewData)
        {
            return viewData[DefaultTimeFormatAlias]?.ToString();
        }

        public static string GetDefaultDateTimeFormat(this ViewDataDictionary viewData)
        {
            return viewData[DefaultDateTimeFormatAlias]?.ToString();
        }

        public static string GetDefaultDatePickerFormat(this ViewDataDictionary viewData)
        {
            return viewData[DefaultDatePickerFormatAlias]?.ToString();
        }

        public static string GetDefaultDateTimePickerFormat(this ViewDataDictionary viewData)
        {
            return viewData[DefaultDateTimePickerFormatAlias]?.ToString();
        }

        public static string GetDefaultDateTimeValueFormat(this ViewDataDictionary viewData)
        {
            return viewData[DefaultDateTimeValuePickerFormatAlias]?.ToString();
        }
    }
}
