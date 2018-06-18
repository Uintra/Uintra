using System.Web.Mvc;
using System.Web.Routing;
using FluentScheduler;
using uIntra.Bulletins;
using uIntra.Core.Jobs;
using uIntra.Events.Dashboard;
using uIntra.Groups.Dashboard;
using uIntra.News.Dashboard;
using uIntra.Notification.Dashboard;
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