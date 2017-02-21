using System.Web;
using System.Web.Mvc;
using uCommunity.Core.App_Plugins.Core.Localization;

namespace uCommunity.Core.App_Plugins.Core.Extentions
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
    }
}