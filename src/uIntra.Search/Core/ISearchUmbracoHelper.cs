using Umbraco.Core.Models;

namespace uIntra.Search.Core
{
    public interface ISearchUmbracoHelper
    {
        IPublishedContent GetSearchPage();

        bool IsSearchable(IPublishedContent content);
    }
}