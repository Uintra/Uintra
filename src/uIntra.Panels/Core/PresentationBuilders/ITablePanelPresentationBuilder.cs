using Uintra.Panels.Core.Models.Table;

namespace Uintra.Panels.Core.PresentationBuilders
{
    public interface ITablePanelPresentationBuilder
    {
        TablePanelViewModel Get(TableEditorModel model);
    }
}