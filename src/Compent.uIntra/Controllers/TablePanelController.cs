using uIntra.Panels.Core.PresentationBuilders;
using uIntra.Panels.Web;
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