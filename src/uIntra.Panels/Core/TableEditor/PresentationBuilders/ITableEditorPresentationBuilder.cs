using uIntra.Panels.Core.TableEditor.Models;

namespace uIntra.Panels.Core.TableEditor.PresentationBuilders
{
    public interface ITableEditorPresentationBuilder
    {
        TableEditorViewModel Map(TableEditorModel model);
    }
}