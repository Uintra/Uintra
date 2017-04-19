using Compent.uCommunity.Core.Helpers;
using uCommunity.Core.Configuration;
using uCommunity.Navigation.Core;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Compent.uCommunity.Core.Navigation
{
    public class UcommunityLeftSideNavigationModelBuilder : LeftSideNavigationModelBuilder
    {
        private readonly IUmbracoContentHelper _umbracoContentHelper;

        public UcommunityLeftSideNavigationModelBuilder(
            UmbracoHelper umbracoHelper,
            IConfigurationProvider<NavigationConfiguration> navigationConfigurationProvider,
            IUmbracoContentHelper umbracoContentHelper)
            : base(umbracoHelper, navigationConfigurationProvider)
        {
            _umbracoContentHelper = umbracoContentHelper;
        }

        protected override bool IsHideFromNavigation(IPublishedContent publishedContent)
        {
            var result = !_umbracoContentHelper.IsContentAvailable(publishedContent);
            return result || base.IsHideFromNavigation(publishedContent);
        }
    }
}