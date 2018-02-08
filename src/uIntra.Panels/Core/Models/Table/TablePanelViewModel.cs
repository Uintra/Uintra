namespace uIntra.Panels.Core.Models.Table
{
    public class TablePanelViewModel
    {
        public string Title { get; set; }

        public TableEditorViewModel Table { get; set; }
        public PaneBehaviorViewModel PanelBehavior { get; set; }
        public PanelColorsViewModel PanelColors { get; set; }
    }
}