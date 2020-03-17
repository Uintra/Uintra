using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Core.Search.Helpers
{
    public interface ISearchUmbracoHelper
    {
        IPublishedContent GetSearchPage();

        bool IsSearchable(IPublishedContent content);

        string GetSearchLink(string searchQuery);
    }
}