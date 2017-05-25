using System.Web;
using System.Web.Mvc;
using Compent.uCommunity.Core;
using uCommunity.Core.Localization;

namespace uCommunity.Core.Extentions
{
    public static class HtmlExtentions
    {
        public static string Localize(this HtmlHelper htmlHelper, string key)
        {
            var translationService = HttpContext.Current.GetService<IIntranetLocalizationService>();
            return translationService.Translate(key);
        }

        public static string Localize<T>(this HtmlHelper htmlHelper, T source)
            where T : struct
        {
            var key = $"{typeof(T).Name}.{source}";
            var translationService = HttpContext.Current.GetService<IIntranetLocalizationService>();
            return translationService.Translate(key);
        }

        public static string GetDateFormat(this HtmlHelper htmlHelper)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();

            return dateTimeFormatProvider.DateFormat;
        }

        public static string GetTimeFormat(this HtmlHelper htmlHelper)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();

            return dateTimeFormatProvider.TimeFormat;
        }

        public static string GetDateTimeFormat(this HtmlHelper htmlHelper)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();

            return dateTimeFormatProvider.DateTimeFormat;
        }

        public static string GetDatePickerFormat(this HtmlHelper htmlHelper)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();

            return dateTimeFormatProvider.DatePickerFormat;
        }

        public static string GetDateTimePickerFormat(this HtmlHelper htmlHelper)
        {

            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();

            return dateTimeFormatProvider.DateTimePickerFormat;
        }

        public static string GetDateTimeValueFormat(this HtmlHelper htmlHelper)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();

            return dateTimeFormatProvider.DateTimeValuePickerFormat;
        }
    }
}