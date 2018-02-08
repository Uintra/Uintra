using System.Web.Mvc;
using System.Web.Mvc.Html;
using uIntra.Panels.Core.Models.Table;

namespace uIntra.Panels.Core.HtmlHelpers
{
    public static class TableComponent
    {
        public static string ViewPath => @"~/App_Plugins/Panels/TableEditor/TableEditor.cshtml";

        public static MvcHtmlString Table(this HtmlHelper html, TableEditorViewModel model)
        {
            if (model.Cells.Count == 0)
            {
                return MvcHtmlString.Empty;
            }

            return html.Partial(ViewPath, model);
        }
    }
}