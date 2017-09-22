using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using uIntra.Core.Links;
using uIntra.Core.Localization;
using uIntra.Core.ModelBinders;

namespace uIntra.Core.Extentions
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

        public static MvcHtmlString PassLinks(this HtmlHelper helper, IActivityLinks links)
        {
            string result = String.Empty;

            result += helper.PassLinks((IActivityCreateLinks)links);
            result += helper.Hidden(LinksBinder.EditFormKey, links.Edit);
            result += helper.Hidden(LinksBinder.DetailsFormKey, links.Details);

            return MvcHtmlString.Create(result);
            
        }

        public static MvcHtmlString PassLinks(this HtmlHelper helper, IActivityCreateLinks links)
        {
            string result = String.Empty;

            result += helper.Hidden(LinksBinder.DetailsNoIdFormKey, links.DetailsNoId);
            result += helper.Hidden(LinksBinder.CreateFormKey, links.Create);
            result += helper.Hidden(LinksBinder.CreatorFormKey, links.Creator);
            result += helper.Hidden(LinksBinder.OverviewFormKey, links.Overview);

            return MvcHtmlString.Create(result);
        }
    }
}