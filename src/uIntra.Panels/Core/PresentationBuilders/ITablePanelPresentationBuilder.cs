using uIntra.Panels.Core.Models.Table;

namespace uIntra.Panels.Core.PresentationBuilders
{
    public interface ITablePanelPresentationBuilder
    {
        TablePanelViewModel Get(TableEditorModel model);
    }
}