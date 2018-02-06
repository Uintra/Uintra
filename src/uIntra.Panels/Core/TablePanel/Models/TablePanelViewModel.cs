using uIntra.Panels.Core.TableEditor.Models;

namespace uIntra.Panels.Core.TablePanel.Models
{
    public class TablePanelViewModel
    {
        public string Title { get; set; }

        public TableEditorViewModel Table { get; set; }
        public TableBehaviorViewModel PanelBehavior { get; set; }
        public TablePanelColorsViewModel PanelColors { get; set; }
    }
}