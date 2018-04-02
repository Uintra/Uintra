using Uintra.Panels.Core.PresentationBuilders;
using Uintra.Panels.Web;
using Umbraco.Web;

namespace Compent.uIntra.Controllers
{
    public class TablePanelController: TablePanelControllerBase
    {
        public TablePanelController(ITablePanelPresentationBuilder tablePanelPresentationBuilder, UmbracoHelper umbracoHelper) : base(tablePanelPresentationBuilder, umbracoHelper)
        {
        }
    }
}