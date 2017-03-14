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

        private List<string> _excludeList;
        protected List<string> ExcludeList => _excludeList ?? (_excludeList = NavigationConfiguration.Exclude);
        protected IPublishedContent CurrentPage => _umbracoHelper.AssignedContentItem;

        public abstract T GetMenu();

        protected abstract bool IsSpecificContent(IPublishedContent publishedContent);

        protected virtual bool IsContentVisible(IPublishedContent publishedContent)
        {
            return true;
        }

        protected virtual bool IsShowInNavigation(IPublishedContent publishedContent)
        {
            return !IsHideInNavigation(publishedContent);
        }

        protected virtual bool IsHideInNavigation(IPublishedContent publishedContent)
        {
            var result = publishedContent.GetPropertyValue<bool?>(NavigationConfiguration.IsHideFromNavigation.Alias);
            return result ?? NavigationConfiguration.IsHideFromNavigation.DefaultValue;
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
            return IsContentVisible(publishedContent) && IsShowInNavigation(publishedContent);
        }

        protected virtual bool IsAddInSpecificContent(IPublishedContent publishedContent)
        {
            return IsContentAvailable(publishedContent) && IsSpecificContent(publishedContent);
        }

        protected virtual IEnumerable<IPublishedContent> GetAvailableContent(IEnumerable<IPublishedContent> publishedContents)
        {
            return publishedContents
                .Where(pContent => !ExcludeList.Contains(pContent.DocumentTypeAlias))
                .Where(IsContentAvailable);
        }

        protected virtual IEnumerable<IPublishedContent> GetSpecificContent(IEnumerable<IPublishedContent> publishedContents)
        {
            return GetAvailableContent(publishedContents)
                .Where(IsSpecificContent);
        }
    }
}
