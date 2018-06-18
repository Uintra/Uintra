using System.Web.Mvc;
using System.Web.Routing;
using uIntra.Bulletins;
using uIntra.Events.Dashboard;
using uIntra.News.Dashboard;
using Umbraco.Core;

namespace uIntra
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
            BulletinsSection.AddSectionToAllUsers(applicationContext);
             
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