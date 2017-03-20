using System.Web.Mvc;
using System.Web.Optimization;
using Compent.uCommunity.App_Start;
using uCommunity.News.Dashboard;
using Umbraco.Core;

namespace Compent.uCommunity
{
    public class Global : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MapperConfig.RegisterMappings();
        }

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            NewsSection.AddSectionToAllUsers(applicationContext);

            base.ApplicationStarted(umbracoApplication, applicationContext);
        }
    }
}
