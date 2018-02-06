using uIntra.Panels.Core.TableEditor.Models;
using Umbraco.Core.Models;

namespace uIntra.Panels.Core.TableEditor.ModelBuilders
{
    public interface ITableEditorModelBuilder
    {
        TableEditorModel Get(IPublishedContent publishedContent, string alias);
    }
}