using System.Configuration;
using System.Web.Configuration;
using uIntra.Core.Extentions;
using uIntra.Core.Installer;
using uIntra.Navigation.Installer;
using uIntra.Notification.Installer;
using uIntra.Search.Installer;
using uIntra.Users.Installer;
using Umbraco.Core;

namespace Compent.uIntra.Installer
{
    public class Installer : ApplicationEventHandler
    {
        private const string UIntraVersion = "1.0.0";

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var uIntraConfigurationStatus = ConfigurationManager.AppSettings[CoreInstallationConstants.AppSettingKey.UIntraConfigurationStatus];
            if (uIntraConfigurationStatus.IsNullOrEmpty())
            {
                InitMigration();
                UpdateUIntraConfigurationStatus();
            }
            
        }

        private void InitMigration()
        {
            var installer = new IntranetInstaller();
            var umbracoContentMigration = new UmbracoContentMigration();
            var defaultLocalizationsMigration = new DefaultLocalizationsMigration();

            installer.Install();
            InheritNavigationCompositions();
            umbracoContentMigration.Init();
            defaultLocalizationsMigration.Init();
        }

        private void InheritNavigationCompositions()
        {
            var nav = NavigationInstallationConstants.DocumentTypeAliases.NavigationComposition;

            CoreInstallationStep.InheritCompositionForPage(CoreInstallationConstants.DocumentTypeAliases.HomePage, nav);
            CoreInstallationStep.InheritCompositionForPage(CoreInstallationConstants.DocumentTypeAliases.ContentPage, nav);
            CoreInstallationStep.InheritCompositionForPage(CoreInstallationConstants.DocumentTypeAliases.ErrorPage, nav);

            CoreInstallationStep.InheritCompositionForPage(SearchInstallationConstants.DocumentTypeAliases.SearchResultPage, nav);
            CoreInstallationStep.InheritCompositionForPage(NotificationInstallationConstants.DocumentTypeAliases.NotificationPage, nav);

            CoreInstallationStep.InheritCompositionForPage(UsersInstallationConstants.DocumentTypeAliases.ProfilePage, nav);
            CoreInstallationStep.InheritCompositionForPage(UsersInstallationConstants.DocumentTypeAliases.ProfileEditPage, nav);
        }

        private void UpdateUIntraConfigurationStatus()
        {
            var webConfigApp = WebConfigurationManager.OpenWebConfiguration("~");
            webConfigApp.AppSettings.Settings[CoreInstallationConstants.AppSettingKey.UIntraConfigurationStatus].Value = UIntraVersion;
            webConfigApp.Save();
        }
    }
}