using System;
using System.Reflection;
using System.Web.Mvc;
using uIntra.Core.Extentions;
using uIntra.Core.Installer;
using uIntra.Core.Installer.Migrations;
using uIntra.Core.MigrationHistories;
using uIntra.Navigation.Installer;
using uIntra.Notification.Installer;
using uIntra.Search.Installer;
using uIntra.Users.Installers;
using Umbraco.Core;

namespace Compent.uIntra.Installer
{
    public class MainInstaller : ApplicationEventHandler
    {
        private readonly Version UIntraVersion = Assembly.GetExecutingAssembly().GetName().Version;

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var migrationHistoryService = DependencyResolver.Current.GetService<IMigrationHistoryService>();
            var lastMigrationHistory = migrationHistoryService.GetLast();

            var installedVersion = lastMigrationHistory != null ? new Version(lastMigrationHistory.Version) : new Version("0.0.0.0");

            var installer = new IntranetInstaller();
            installer.Install(installedVersion, UIntraVersion);

            if (lastMigrationHistory == null)
            {
                InitMigration();
            }

            if (UIntraVersion > installedVersion)
            {
                migrationHistoryService.Create(UIntraVersion.ToString());
            }
        }

        private void InitMigration()
        {
            var umbracoContentMigration = new UmbracoContentMigration();
            var defaultLocalizationsMigration = new DefaultLocalizationsMigration();

            InheritNavigationCompositions();
            umbracoContentMigration.Init();
            defaultLocalizationsMigration.Init();

            AddDefaultBackofficeSectionsToAdmin();
        }

        private void InheritNavigationCompositions()
        {
            var nav = NavigationInstallationConstants.DocumentTypeAliases.NavigationComposition;

            CoreInstallationStep_0_0_1.InheritCompositionForPage(CoreInstallationConstants.DocumentTypeAliases.HomePage, nav);
            CoreInstallationStep_0_0_1.InheritCompositionForPage(CoreInstallationConstants.DocumentTypeAliases.ContentPage, nav);
            CoreInstallationStep_0_0_1.InheritCompositionForPage(CoreInstallationConstants.DocumentTypeAliases.ErrorPage, nav);

            CoreInstallationStep_0_0_1.InheritCompositionForPage(SearchInstallationConstants.DocumentTypeAliases.SearchResultPage, nav);
            CoreInstallationStep_0_0_1.InheritCompositionForPage(NotificationInstallationConstants.DocumentTypeAliases.NotificationPage, nav);

            CoreInstallationStep_0_0_1.InheritCompositionForPage(UsersInstallationConstants.DocumentTypeAliases.ProfilePage, nav);
            CoreInstallationStep_0_0_1.InheritCompositionForPage(UsersInstallationConstants.DocumentTypeAliases.ProfileEditPage, nav);
        }

        private void AddDefaultBackofficeSectionsToAdmin()
        {
            var userService = ApplicationContext.Current.Services.UserService;

            userService.AddSectionToAllUsers("news", UsersInstallationConstants.DefaultMember.UmbracoAdminUserId);
            userService.AddSectionToAllUsers("events", UsersInstallationConstants.DefaultMember.UmbracoAdminUserId);
            userService.AddSectionToAllUsers("bulletins", UsersInstallationConstants.DefaultMember.UmbracoAdminUserId);
            userService.AddSectionToAllUsers("SentMails", UsersInstallationConstants.DefaultMember.UmbracoAdminUserId);
            userService.AddSectionToAllUsers("Localization", UsersInstallationConstants.DefaultMember.UmbracoAdminUserId);
        }
    }
}