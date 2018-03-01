using uIntra.Panels.Core.Models.Table;

namespace UIntra.Panels.Core.PresentationBuilders
{
    public interface ITablePanelPresentationBuilder
    {
        TablePanelViewModel Get(TableEditorModel model);
    }
}