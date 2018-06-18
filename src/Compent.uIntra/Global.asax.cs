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
            NewsSection.AddSectionToAllUsers(applicationContext);
            EventsSection.AddSectionToAllUsers(applicationContext);
            BulletinsSection.AddSectionToAllUsers(applicationContext);
            GroupsSection.AddSectionToAllUsers(applicationContext);
            NotificationSettingsSection.AddSectionToAllUsers(applicationContext);

            RegisterRoutes();

            JobManager.JobFactory = DependencyResolver.Current.GetService<IJobFactory>();
            JobManager.Initialize(new JobsRegistry());            

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