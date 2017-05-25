using System.Web.Mvc;
using System.Web.Routing;
using uCommunity.Events.Dashboard;
using uCommunity.News.Dashboard;
using Umbraco.Core;

namespace Compent.uIntra
{
    public class Global : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            AreaRegistration.RegisterAllAreas();
            MapperConfig.RegisterMappings();
        }

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            NewsSection.AddSectionToAllUsers(applicationContext);
            EventsSection.AddSectionToAllUsers(applicationContext);
            RegisterRoutes();

            base.ApplicationStarted(umbracoApplication, applicationContext);
        }

        private void RegisterRoutes()
        {
            RouteTable.Routes.MapRoute(
                "login",
                "Login/{action}",
                new
                {
                    controller = "Login",
                    action = "Login"
                });
        }
    }
}