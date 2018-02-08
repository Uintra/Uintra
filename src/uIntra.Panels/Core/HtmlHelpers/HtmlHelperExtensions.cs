using System.Web.Mvc;
using System.Web.Mvc.Html;
using uIntra.Panels.Core.Models;
using Umbraco.Core;

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

        public static MvcHtmlString PanelBehaviour(this HtmlHelper html, PaneBehaviorViewModel panelBehavior)
        {
            return new MvcHtmlString(panelBehavior?.Behavior ?? string.Empty);
        }

        public static MvcHtmlString PanelColors(this HtmlHelper html, PanelColorsViewModel panelColors)
        {
            if (panelColors == null)
            {
                return new MvcHtmlString(string.Empty);
            }

            var sectionStyles =
                (panelColors.BackgroundColor.IsNullOrWhiteSpace() ? "" : "background-color:" + panelColors.BackgroundColor) + ";" +
                (panelColors.TextColor.IsNullOrWhiteSpace() ? "" : "color:" + panelColors.TextColor) + ";";

            return new MvcHtmlString(sectionStyles);
        }

    }
}
