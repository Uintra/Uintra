using uIntra.Panels.Core.Models;
using uIntra.Panels.Core.Models.Table;
using Umbraco.Core.Models;

namespace uIntra.Panels.Core.ModelBuilders
{
    
    public interface ITablePanelModelBuilder
    {
        TablePanelModel Get(IPublishedContent publishedContent);

        string GetTitleAlias();
        string GetTableAlias();

        string ContentTypeAlias { get; }
    }
}