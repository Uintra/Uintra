using Umbraco.Core;

namespace uCommunity.Core.Migrations
{
    public class MigrationsHandler: ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            MediaMigrations.Migrate();
        }
    }
}