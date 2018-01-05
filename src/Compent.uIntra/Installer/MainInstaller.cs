using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web.Mvc;
using Compent.uIntra.Installer.Migrations;
using EmailWorker.Data.Services.Interfaces;
using uIntra.Bulletins;
using uIntra.Bulletins.Installer;
using uIntra.Core;
using uIntra.Core.Activity;
using Extensions;
using uIntra.Core.Installer;
using uIntra.Core.MigrationHistories;
using uIntra.Core.User;
using uIntra.Events;
using uIntra.Events.Installer;
using uIntra.Groups.Installer;
using uIntra.Navigation.Installer;
using uIntra.News;
using uIntra.News.Installer;
using uIntra.Notification.Configuration;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Compent.uIntra.Installer
{
    public class MainInstaller : ApplicationEventHandler
    {
        private readonly Version UIntraVersion = Assembly.GetExecutingAssembly().GetName().Version;
        private readonly Version NewPluginsUIntraVersion = new Version("0.2.0.8");
        private readonly Version AddingHeadingUIntraVersion = new Version("0.2.2.10");
        private readonly Version AddingOwnerUIntraVersion = new Version("0.2.4.0");
        private readonly Version DeleteMailTemplates = new Version("0.2.5.6");
        private readonly Version PagePromotionUIntraVersion = new Version("0.2.8.0");
        private readonly Version EventsPublishDateUIntraVersion = new Version("0.2.12.0");
        private readonly Version TaggingUIntraVersion = new Version("0.2.13.0");

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            SetCurrentCulture();
            AddDefaultMailSettings();
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
                InstallationStepsHelper.InheritCompositionForPage(
                    CoreInstallationConstants.DocumentTypeAliases.Heading,
                    NavigationInstallationConstants.DocumentTypeAliases.NavigationComposition);
            }

            if (installedVersion < AddingOwnerUIntraVersion && UIntraVersion >= AddingOwnerUIntraVersion)
            {
                FixEmptyOwners();
            }

            if (installedVersion != new Version("0.0.0.0") && installedVersion < DeleteMailTemplates && UIntraVersion >= DeleteMailTemplates)
            {
                var notificationSettingsMigrations = new NotificationSettingsMigrations();
                notificationSettingsMigrations.Execute();

                var uiNotificationMigration = DependencyResolver.Current.GetService<OldUiNotificationMigration>();
                uiNotificationMigration.Execute();
            }


            if (installedVersion < DeleteMailTemplates && UIntraVersion >= DeleteMailTemplates)
            {
                DeleteExistedMailTemplates();
            }

            if (installedVersion < PagePromotionUIntraVersion && UIntraVersion >= PagePromotionUIntraVersion)
            {
                var pagePromotionMigration = new PagePromotionMigration();
                pagePromotionMigration.Execute();

                InstallationStepsHelper.InheritCompositionForPage(
                    CoreInstallationConstants.DocumentTypeAliases.ContentPage,
                    PagePromotionInstallationConstants.DocumentTypeAliases.PagePromotionComposition);
            }

            if (installedVersion < EventsPublishDateUIntraVersion && UIntraVersion >= EventsPublishDateUIntraVersion)
            {
                var eventPublishDateMigration = new EventPublishDateMigration();
                eventPublishDateMigration.Execute();
            }

            if (installedVersion < TaggingUIntraVersion && UIntraVersion >= TaggingUIntraVersion)
            {
                var taggingMigration = new TaggingMigration();
                taggingMigration.Execute();
            }

            if (UIntraVersion > installedVersion)
            {
                migrationHistoryService.Create(UIntraVersion.ToString());
            }

            AddDefaultMailSettings();



        }

        private static void SetCurrentCulture()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
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

        private void DeleteExistedMailTemplates()
        {
            var contentService = DependencyResolver.Current.GetService<IContentService>();
            var _umbracoHelper = DependencyResolver.Current.GetService<UmbracoHelper>();
            var _documentTypeAliasProvider = DependencyResolver.Current.GetService<IDocumentTypeAliasProvider>();

            var mailTemplateFolderXpath = XPathHelper.GetXpath(_documentTypeAliasProvider.GetDataFolder(), _documentTypeAliasProvider.GetMailTemplateFolder());
            var publishedContent = _umbracoHelper.TypedContentSingleAtXPath(mailTemplateFolderXpath);

            bool IsForRemove(IPublishedContent content)
            {
                var templateType = content.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName);

                return templateType.In(
                    NotificationTypeEnum.Event,
                    NotificationTypeEnum.EventUpdated,
                    NotificationTypeEnum.EventHided,
                    NotificationTypeEnum.BeforeStart,
                    NotificationTypeEnum.News,
                    NotificationTypeEnum.Idea,
                    NotificationTypeEnum.ActivityLikeAdded,
                    NotificationTypeEnum.CommentAdded,
                    NotificationTypeEnum.CommentEdited,
                    NotificationTypeEnum.CommentReplied);
            }

            var publishedContentToRemove = publishedContent.Children.Where(IsForRemove);
            var contentToRemove = contentService.GetByIds(publishedContentToRemove.Select(c => c.Id)).ToList();
            contentToRemove.ForEach(c => contentService.Delete(c));
        }

        private void AllowActivitiesForGroups()
        {
            var groupRoomPageAlias = GroupsInstallationConstants.DocumentTypeAliases.GroupsRoomPage;

            InstallationStepsHelper.AddAllowedChildNode(groupRoomPageAlias, NewsInstallationConstants.DocumentTypeAliases.NewsOverviewPage);
            InstallationStepsHelper.AddAllowedChildNode(groupRoomPageAlias, EventsInstallationConstants.DocumentTypeAliases.EventsOverviewPage);
            InstallationStepsHelper.AddAllowedChildNode(groupRoomPageAlias, BulletinsInstallationConstants.DocumentTypeAliases.BulletinsOverviewPage);
        }

        private void InheritNavigationCompositions()
        {
            var nav = NavigationInstallationConstants.DocumentTypeAliases.NavigationComposition;

            InstallationStepsHelper.InheritCompositionForPage(CoreInstallationConstants.DocumentTypeAliases.HomePage, nav);
            InstallationStepsHelper.InheritCompositionForPage(CoreInstallationConstants.DocumentTypeAliases.ContentPage, nav);
            InstallationStepsHelper.InheritCompositionForPage(CoreInstallationConstants.DocumentTypeAliases.Heading, nav);

            InstallationStepsHelper.InheritCompositionForPage(GroupsInstallationConstants.DocumentTypeAliases.GroupsCreatePage, nav);
            InstallationStepsHelper.InheritCompositionForPage(GroupsInstallationConstants.DocumentTypeAliases.GroupsDeactivatedGroupPage, nav);
            InstallationStepsHelper.InheritCompositionForPage(GroupsInstallationConstants.DocumentTypeAliases.GroupsDocumentsPage, nav);
            InstallationStepsHelper.InheritCompositionForPage(GroupsInstallationConstants.DocumentTypeAliases.GroupsEditPage, nav);
            InstallationStepsHelper.InheritCompositionForPage(GroupsInstallationConstants.DocumentTypeAliases.GroupsMembersPage, nav);
            InstallationStepsHelper.InheritCompositionForPage(GroupsInstallationConstants.DocumentTypeAliases.GroupsMyGroupsOverviewPage, nav);
            InstallationStepsHelper.InheritCompositionForPage(GroupsInstallationConstants.DocumentTypeAliases.GroupsRoomPage, nav);
        }

        private void AddDefaultBackofficeSectionsToAdmin()
        {
            var userService = ApplicationContext.Current.Services.UserService;
            var adminUserGroup = userService.GetAllUserGroups().Single(group => group.Alias == "admin");

            adminUserGroup.AddAllowedSection("SentMails");
            adminUserGroup.AddAllowedSection("Localization");

            userService.Save(adminUserGroup);
        }

        private void AddDefaultMailSettings()
        {
            var mailsColumnSettingsService = DependencyResolver.Current.GetService<ISentMailsColumnSettingsService>();
            var sentMailsSmtpSettingsService = DependencyResolver.Current.GetService<ISentMailsSmtpSettingsService>();
            mailsColumnSettingsService.CreateColumnDefaultSettings();
            sentMailsSmtpSettingsService.CreateSmtpDefaultSettings();
        }

        private void FixEmptyOwners()
        {
            var userService = DependencyResolver.Current.GetService<IIntranetUserService<IIntranetUser>>();

            var activityServices = DependencyResolver.Current
                .GetServices<IIntranetActivityService<IIntranetActivity>>()
                .Where(service => service.ActivityType.Id != (int)IntranetActivityTypeEnum.PagePromotion);
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