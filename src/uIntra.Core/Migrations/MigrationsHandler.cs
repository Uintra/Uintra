using Umbraco.Core;

namespace uIntra.Core.Migrations
{
    public class MigrationsHandler: ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            //MediaMigrations.Migrate();
        }
    }
}