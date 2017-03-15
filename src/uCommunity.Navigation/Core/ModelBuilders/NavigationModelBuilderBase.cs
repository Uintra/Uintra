using System.Collections.Generic;
using System.Linq;
using uCommunity.Core.Configuration;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uCommunity.Navigation.Core
{
    public abstract class NavigationModelBuilderBase<T> where T : class
    {
        private readonly UmbracoHelper _umbracoHelper;

        protected readonly NavigationConfiguration NavigationConfiguration;

        protected NavigationModelBuilderBase(
            UmbracoHelper umbracoHelper,
            IConfigurationProvider<NavigationConfiguration> navigationConfigurationProvider
            )
        {
            _umbracoHelper = umbracoHelper;
            NavigationConfiguration = navigationConfigurationProvider.GetSettings();
        }

        protected IPublishedContent CurrentPage => _umbracoHelper.AssignedContentItem;

        public abstract T GetMenu();

        protected abstract bool IsHideFromNavigation(IPublishedContent publishedContent);

        protected virtual bool IsContentVisible(IPublishedContent publishedContent)
        {
            return true;
        }

        protected virtual bool IsShowInNavigation(IPublishedContent publishedContent)
        {
            return !IsHideFromNavigation(publishedContent);
        }

        protected virtual string GetNavigationName(IPublishedContent publishedContent)
        {
            var result = publishedContent.GetPropertyValue<string>(NavigationConfiguration.NavigationName.Alias);
            return string.IsNullOrEmpty(result) ? publishedContent.Name : result;
        }

        protected virtual bool IsContentUnavailable(IPublishedContent publishedContent)
        {
            return !IsContentAvailable(publishedContent);
        }

        protected virtual bool IsContentAvailable(IPublishedContent publishedContent)
        {
            var isNavigationItem = publishedContent.HasProperty(NavigationConfiguration.NavigationName.Alias);
            return isNavigationItem && IsContentVisible(publishedContent) && IsShowInNavigation(publishedContent);
        }

        protected virtual IEnumerable<IPublishedContent> GetAvailableContent(IEnumerable<IPublishedContent> publishedContents)
        {
            return publishedContents
                .Where(pContent => !NavigationConfiguration.Exclude.Contains(pContent.DocumentTypeAlias))
                .Where(IsContentAvailable);
        }
    }
}
