using Uintra.Panels.Core.PresentationBuilders;
using Uintra.Panels.Web;

namespace Compent.Uintra.Controllers
{
    public class TablePanelController : TablePanelControllerBase
    {
        public TablePanelController(ITablePanelPresentationBuilder tablePanelPresentationBuilder)
            : base(tablePanelPresentationBuilder)
        {
        }
    }
}