using UBaseline.Shared.SearchPage;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Core.Search.Helpers
{
    public interface ISearchUmbracoHelper
    {
        SearchPageModel GetSearchPage();

        bool IsSearchable(IPublishedContent content);

        string GetSearchLink(string searchQuery);
    }
}