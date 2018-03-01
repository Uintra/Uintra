using System.Web.Mvc;
using System.Web.Mvc.Html;
using Uintra.Panels.Core.Models.Table;

namespace Uintra.Panels.Core.HtmlHelpers
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