using uIntra.Panels.Core.TablePanel.Models;
using Umbraco.Core.Models;

namespace uIntra.Panels.Core.TablePanel.PresentationBuilders
{    
    public interface ITablePanelPresentationBuilder 
    {
        TablePanelViewModel Get(IPublishedContent publishedContent);

        string ContentTypeAlias { get; }
    }
}