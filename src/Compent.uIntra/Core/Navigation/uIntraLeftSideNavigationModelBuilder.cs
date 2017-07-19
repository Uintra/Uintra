using Compent.uIntra.Core.Helpers;
using uIntra.Core.Configuration;
using uIntra.Navigation;
using uIntra.Navigation.Configuration;
using Umbraco.Core.Models;
using Umbraco.Web;
using System.Web;

namespace Compent.uIntra.Core.Navigation
{
    public class UintraLeftSideNavigationModelBuilder : LeftSideNavigationModelBuilder
    {
        private readonly IUmbracoContentHelper _umbracoContentHelper;

        public UintraLeftSideNavigationModelBuilder(
            HttpContext context,
            UmbracoHelper umbracoHelper,
            IConfigurationProvider<NavigationConfiguration> navigationConfigurationProvider,
            IUmbracoContentHelper umbracoContentHelper)
            : base(context, umbracoHelper, navigationConfigurationProvider)
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