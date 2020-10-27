using UBaseline.Shared.SearchPage;
using Uintra.Core.Search.Entities;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra.Core.Search.Helpers
{
    public interface ISearchUmbracoHelper
    {
        SearchPageModel GetSearchPage();

        bool IsSearchable(IPublishedContent content);

        string GetSearchLink(string searchQuery);
        SearchableContent GetContent(IPublishedContent publishedContent);
    }
}