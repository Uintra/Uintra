using uIntra.Panels.Core.TablePanel.Models;
using Umbraco.Core.Models;

namespace uIntra.Panels.Core.TablePanel.ModelBuilders
{
    
    public interface ITablePanelModelBuilder
    {
        TablePanelModel Get(IPublishedContent publishedContent);

        string GetTitleAlias();
        string GetTableAlias();

        string ContentTypeAlias { get; }
    }
}