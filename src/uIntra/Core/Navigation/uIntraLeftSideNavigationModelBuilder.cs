using uIntra.Core.Configuration;
using uIntra.Core.Helpers;
using uIntra.Navigation;
using uIntra.Navigation.Configuration;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uIntra.Core.Navigation
{
    public class UintraLeftSideNavigationModelBuilder : LeftSideNavigationModelBuilder
    {
        private readonly IUmbracoContentHelper _umbracoContentHelper;

        public UintraLeftSideNavigationModelBuilder(
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