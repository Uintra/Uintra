using System.Web.Mvc;
using System.Web.Routing;
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
            MapperConfig.RegisterMappings();
        }

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            NewsSection.AddSectionToAllUsers(applicationContext);
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