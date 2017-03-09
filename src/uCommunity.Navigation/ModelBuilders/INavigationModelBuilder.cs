using Umbraco.Core.Models;

namespace uCommunity.Navigation
{
    public interface INavigationModelBuilder
    {
        MenuViewModel GetLeftSideMenu();
        string GetNavigationName(IPublishedContent publishedContent);
        bool IsContentVisible(IPublishedContent publishedContent);
        bool IsHideInNavigation(IPublishedContent publishedContent);
        bool IsShowInHomeNavigation(IPublishedContent publishedContent);
        bool IsShowNavigation(IPublishedContent publishedContent);
    }
}