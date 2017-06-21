using Umbraco.Core.Models;

namespace uIntra.Search.Core
{
    public interface ISearchUmbracoHelper
    {
        IPublishedContent GetSearchPage();

        IPublishedContent GetIndexerPage();

        bool IsSearchable(IPublishedContent content);
    }
}