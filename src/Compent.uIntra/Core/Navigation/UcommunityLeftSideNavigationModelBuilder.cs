using Compent.uIntra.Core.Helpers;
using uIntra.Core.Configuration;
using uIntra.Navigation;
using uIntra.Navigation.Configuration;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Compent.uIntra.Core.Navigation
{
    public class uIntraLeftSideNavigationModelBuilder : LeftSideNavigationModelBuilder
    {
        private readonly IUmbracoContentHelper _umbracoContentHelper;

        public uIntraLeftSideNavigationModelBuilder(
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