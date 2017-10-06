using System;
using System.Reflection;
using System.Web.Mvc;
using uIntra.Bulletins.Installer;
using uIntra.Core.Installer;
using uIntra.Core.Installer.Migrations;
using uIntra.Core.MigrationHistories;
using uIntra.Events.Installer;
using uIntra.Groups.Installer;
using uIntra.Navigation.Installer;
using uIntra.News.Installer;
using uIntra.Notification.Installer;
using uIntra.Search.Installer;
using uIntra.Users.Installers;
using Umbraco.Core;

namespace Compent.uIntra.Installer
{
    public class MainInstaller : ApplicationEventHandler
    {
        private readonly Version UIntraVersion = Assembly.GetExecutingAssembly().GetName().Version;
        private readonly Version NewPluginsUIntraVersion = new Version("0.2.0.8");

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var migrationHistoryService = DependencyResolver.Current.GetService<IMigrationHistoryService>();
            var lastMigrationHistory = migrationHistoryService.GetLast();

            var installedVersion = lastMigrationHistory != null ? new Version(lastMigrationHistory.Version) : new Version("0.0.0.0");

            var installer = new IntranetInstaller();
            installer.Install(installedVersion, UIntraVersion);
            InitMigration();

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
            AllowActivitiesForGroups();
            umbracoContentMigration.Init();
            defaultLocalizationsMigration.Init();

            var migrationHistoryService = DependencyResolver.Current.GetService<IMigrationHistoryService>();
            var lastMigrationHistory = migrationHistoryService.GetLast();
            if (lastMigrationHistory != null)
            {
                var lastMigrationVersion = new Version(lastMigrationHistory.Version);
                if (lastMigrationVersion < NewPluginsUIntraVersion)
                {
                    umbracoContentMigration.UpdateActivitiesGrids();
                }
            }

            AddDefaultBackofficeSectionsToAdmin();
        }

        private void AllowActivitiesForGroups()
        {
            var groupRoomPageAlias = GroupsInstallationConstants.DocumentTypeAliases.GroupsRoomPage;

            CoreInstallationStep_0_0_1.AddAllowedChildNode(groupRoomPageAlias, NewsInstallationConstants.DocumentTypeAliases.NewsOverviewPage);
            CoreInstallationStep_0_0_1.AddAllowedChildNode(groupRoomPageAlias, EventsInstallationConstants.DocumentTypeAliases.EventsOverviewPage);
            CoreInstallationStep_0_0_1.AddAllowedChildNode(groupRoomPageAlias, BulletinsInstallationConstants.DocumentTypeAliases.BulletinsOverviewPage);
        }

        private void InheritNavigationCompositions()
        {
            var nav = NavigationInstallationConstants.DocumentTypeAliases.NavigationComposition;
            var homeNav = NavigationInstallationConstants.DocumentTypeAliases.HomeNavigationComposition;

            CoreInstallationStep_0_0_1.InheritCompositionForPage(CoreInstallationConstants.DocumentTypeAliases.HomePage, nav);
            CoreInstallationStep_0_0_1.InheritCompositionForPage(CoreInstallationConstants.DocumentTypeAliases.ContentPage, nav);
            CoreInstallationStep_0_0_1.InheritCompositionForPage(CoreInstallationConstants.DocumentTypeAliases.ErrorPage, nav);

            CoreInstallationStep_0_0_1.InheritCompositionForPage(NewsInstallationConstants.DocumentTypeAliases.NewsOverviewPage, nav);
            CoreInstallationStep_0_0_1.InheritCompositionForPage(EventsInstallationConstants.DocumentTypeAliases.EventsOverviewPage, nav);
            CoreInstallationStep_0_0_1.InheritCompositionForPage(BulletinsInstallationConstants.DocumentTypeAliases.BulletinsOverviewPage, nav);

            CoreInstallationStep_0_0_1.InheritCompositionForPage(NewsInstallationConstants.DocumentTypeAliases.NewsOverviewPage, homeNav);
            CoreInstallationStep_0_0_1.InheritCompositionForPage(EventsInstallationConstants.DocumentTypeAliases.EventsOverviewPage, homeNav);
            CoreInstallationStep_0_0_1.InheritCompositionForPage(BulletinsInstallationConstants.DocumentTypeAliases.BulletinsOverviewPage, homeNav);

            CoreInstallationStep_0_0_1.InheritCompositionForPage(SearchInstallationConstants.DocumentTypeAliases.SearchResultPage, nav);
            CoreInstallationStep_0_0_1.InheritCompositionForPage(NotificationInstallationConstants.DocumentTypeAliases.NotificationPage, nav);

            CoreInstallationStep_0_0_1.InheritCompositionForPage(UsersInstallationConstants.DocumentTypeAliases.ProfilePage, nav);
            CoreInstallationStep_0_0_1.InheritCompositionForPage(UsersInstallationConstants.DocumentTypeAliases.ProfileEditPage, nav);


            CoreInstallationStep_0_0_1.InheritCompositionForPage(GroupsInstallationConstants.DocumentTypeAliases.GroupsCreatePage, nav);
            CoreInstallationStep_0_0_1.InheritCompositionForPage(GroupsInstallationConstants.DocumentTypeAliases.GroupsDeactivatedGroupPage, nav);
            CoreInstallationStep_0_0_1.InheritCompositionForPage(GroupsInstallationConstants.DocumentTypeAliases.GroupsDocumentsPage, nav);
            CoreInstallationStep_0_0_1.InheritCompositionForPage(GroupsInstallationConstants.DocumentTypeAliases.GroupsEditPage, nav);
            CoreInstallationStep_0_0_1.InheritCompositionForPage(GroupsInstallationConstants.DocumentTypeAliases.GroupsMembersPage, nav);
            CoreInstallationStep_0_0_1.InheritCompositionForPage(GroupsInstallationConstants.DocumentTypeAliases.GroupsMyGroupsOverviewPage, nav);
            CoreInstallationStep_0_0_1.InheritCompositionForPage(GroupsInstallationConstants.DocumentTypeAliases.GroupsRoomPage, nav);
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