using uIntra.Panels.Core.ModelBuilders;

namespace uIntra.Panels.Core.Models.Table
{
    public class TablePanelModel
    {
        public string Title { get; set; }

        public TableEditorModel Table { get; set; }
        public PanelBehaviorModel BackgroundBehavior { get; set; }
        public TablePanelColorsModel PanelColors { get; set; }
    }
}
