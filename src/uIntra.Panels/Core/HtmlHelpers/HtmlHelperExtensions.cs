using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace uIntra.Panels.Core.HtmlHelpers
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString PartialNullable<T>(this HtmlHelper html, string partialViewName, T model)
            where T : class
        {
            if (model == null)
            {
                return MvcHtmlString.Empty;
            }

            return html.Partial(partialViewName, model);
        }        
    }
}
