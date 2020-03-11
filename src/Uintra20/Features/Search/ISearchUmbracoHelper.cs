using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Features.Search
{
    public interface ISearchUmbracoHelper
    {
        IPublishedContent GetSearchPage();

        bool IsSearchable(IPublishedContent content);

        string GetSearchLink(string searchQuery);
    }
}