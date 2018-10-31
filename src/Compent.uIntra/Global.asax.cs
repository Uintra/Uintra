using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using FluentScheduler;
using Uintra.Bulletins;
using Uintra.Core.Jobs;
using Uintra.Events.Dashboard;
using Uintra.Groups.Dashboard;
using Uintra.News.Dashboard;
using Uintra.Notification.Dashboard;
using Umbraco.Core;

namespace Compent.Uintra
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
            RegisterRoutes();
            GlobalConfiguration.Configure(WebApiConfig.Register);

            JobManager.JobFactory = DependencyResolver.Current.GetService<IJobFactory>();
            JobManager.Initialize(new JobsRegistry());

            AddSectionsToAllUsers();

            base.ApplicationStarted(umbracoApplication, applicationContext);
        }

        private static void RegisterRoutes()
        {
            RouteTable.Routes.MapRoute(
                "login",
                "Login/{action}",
                new
                {
                    controller = "Login",
                    action = "Login"
                });
            RouteTable.Routes.MapRoute(
                "sync",
                "sync/{action}",
                new
                {
                    controller = "sync",
                    action = "users"
                });

            RouteTable.Routes.MapRoute(
                "authcallback",
                "authcallback/{action}",
                new
                {
                    controller = "authcallback",
                    action = "indexasync"
                });
        }

        private static void AddSectionsToAllUsers()
        {
            NewsSection.AddSectionToAllUsers();
            EventsSection.AddSectionToAllUsers();
            BulletinsSection.AddSectionToAllUsers();
            GroupsSection.AddSectionToAllUsers();
            NotificationSettingsSection.AddSectionToAllUsers();
        }
    }
}