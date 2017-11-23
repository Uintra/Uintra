using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using uIntra.Bulletins;
using uIntra.Bulletins.Installer;
using uIntra.Core.Activity;
using uIntra.Core.Installer;
using uIntra.Core.Installer.Migrations;
using uIntra.Core.MigrationHistories;
using uIntra.Core.User;
using uIntra.Events;
using uIntra.Events.Installer;
using uIntra.Groups.Installer;
using uIntra.Navigation.Installer;
using uIntra.News;
using uIntra.News.Installer;
using Umbraco.Core;

namespace Compent.uIntra.Installer
{
    public class MainInstaller : ApplicationEventHandler
    {
        private readonly Version UIntraVersion = Assembly.GetExecutingAssembly().GetName().Version;
        private readonly Version NewPluginsUIntraVersion = new Version("0.2.0.8");
        private readonly Version AddingHeadingUIntraVersion = new Version("0.2.2.10");
        private readonly Version AddingOwnerUIntraVersion = new Version("0.2.4.0");

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

            if (installedVersion < AddingHeadingUIntraVersion && UIntraVersion >= AddingHeadingUIntraVersion)
            {
                CoreInstallationStep_0_0_1.InheritCompositionForPage(
                    CoreInstallationConstants.DocumentTypeAliases.Heading,
                    NavigationInstallationConstants.DocumentTypeAliases.NavigationComposition);
            }

            if (installedVersion < AddingOwnerUIntraVersion && UIntraVersion >= AddingOwnerUIntraVersion)
            {
                FixEmptyOwners();
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

            CoreInstallationStep_0_0_1.InheritCompositionForPage(CoreInstallationConstants.DocumentTypeAliases.HomePage, nav);
            CoreInstallationStep_0_0_1.InheritCompositionForPage(CoreInstallationConstants.DocumentTypeAliases.ContentPage, nav);
            CoreInstallationStep_0_0_1.InheritCompositionForPage(CoreInstallationConstants.DocumentTypeAliases.Heading, nav);

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
            var adminUserGroup = userService.GetAllUserGroups().Single(group => group.Alias == "admin");

            adminUserGroup.AddAllowedSection("SentMails");
            adminUserGroup.AddAllowedSection("Localization");

            userService.Save(adminUserGroup);
        }

        private void FixEmptyOwners()
        {
            var userService = DependencyResolver.Current.GetService<IIntranetUserService<IIntranetUser>>();

            var activityServices = DependencyResolver.Current.GetServices<IIntranetActivityService<IIntranetActivity>>();
            foreach (var service in activityServices)
            {
                var activities = service.GetAll(true).ToList();

                foreach (var activity in activities)
                {
                    var creator = activity as IHaveCreator;
                    var owner = activity as IHaveOwner;

                    if (creator == null || owner == null || owner.OwnerId != Guid.Empty)
                        continue;

                    var creatorId = creator.CreatorId != Guid.Empty
                        ? creator.CreatorId
                        : userService.Get(creator.UmbracoCreatorId.Value).Id;

                    switch (activity)
                    {
                        case NewsBase news:
                            news.OwnerId = creatorId;
                            service.Save(news);
                            break;

                        case EventBase @event:
                            @event.OwnerId = creatorId;
                            service.Save(@event);
                            break;

                        case BulletinBase bulletin:
                            bulletin.OwnerId = creatorId;
                            service.Save(bulletin);
                            break;
                    }
                }
            }
        }
    }
}