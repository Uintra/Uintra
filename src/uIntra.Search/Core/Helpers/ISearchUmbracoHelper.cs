using Umbraco.Core.Models;

namespace Uintra.Search
{
    public interface ISearchUmbracoHelper
    {
        IPublishedContent GetSearchPage();

        bool IsSearchable(IPublishedContent content);

        string GetSearchLink(string searchQuery);
    }
}