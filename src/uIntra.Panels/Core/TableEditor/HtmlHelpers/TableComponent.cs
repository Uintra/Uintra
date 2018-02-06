using System.Web.Mvc;
using System.Web.Mvc.Html;
using uIntra.Panels.Core.TableEditor.Models;

namespace uIntra.Panels.Core.TableEditor.HtmlHelpers
{
    public static class TableComponent
    {
        public static MvcHtmlString Table(this HtmlHelper html, TableEditorViewModel model)
        {
            if (model.Cells.Count == 0)
            {
                return MvcHtmlString.Empty;
            }

            return html.Partial("~/uPortal/Core/Shared/PropertyEditors/TableEditor/Views/TableEditor.cshtml", model);
        }
    }
}