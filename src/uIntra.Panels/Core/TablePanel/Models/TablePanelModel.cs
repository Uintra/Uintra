using uIntra.Panels.Core.TableEditor.Models;
using uIntra.Panels.Core.TablePanel.ModelBuilders;

namespace uIntra.Panels.Core.TablePanel.Models
{
    public class TablePanelModel
    {
        public string Title { get; set; }

        public TableEditorModel Table { get; set; }
        public PanelBehaviorModel BackgroundBehavior { get; set; }
        public TablePanelColorsModel PanelColors { get; set; }
    }
}
