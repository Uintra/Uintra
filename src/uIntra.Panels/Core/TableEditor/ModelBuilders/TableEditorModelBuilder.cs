using uIntra.Panels.Core.TableEditor.Models;
using Umbraco.Core.Models;

namespace uIntra.Panels.Core.TableEditor.ModelBuilders
{
    public class TableEditorModelBuilder : ITableEditorModelBuilder
    {
        public virtual TableEditorModel Get(IPublishedContent publishedContent, string alias)
        {
            //var prop = propertyProvider.GetValue<string>(publishedContent, alias);
            //var result = prop.Deserialize<TableEditorModel>();
            //return result;

            return null;
        }
    }
}