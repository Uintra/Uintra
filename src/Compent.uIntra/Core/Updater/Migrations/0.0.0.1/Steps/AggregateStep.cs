using System;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Compent.uIntra.Core.Updater.Migrations._0._0._0._1.Constants;
using Compent.uIntra.Core.Updater.Migrations._0._0._0._1.Steps.AggregateSubsteps;
using Compent.uIntra.Installer.Migrations;
using Compent.uIntra.Installer.Migrations.OldSubscribeSettings;
using EmailWorker.Data.Services.Interfaces;
using Extensions;
using uIntra.Bulletins;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.Installer;
using uIntra.Core.MigrationHistories;
using uIntra.Core.User;
using uIntra.Events;
using uIntra.News;
using uIntra.Notification.Configuration;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using static Compent.uIntra.Core.Updater.ExecutionResult;

namespace Compent.uIntra.Core.Updater.Migrations._0._0._0._1.Steps
{
    public class AggregateStep : IMigrationStep
    {
        public ExecutionResult Execute()
        {
            var migrationHistoryService = DependencyResolver.Current.GetService<IMigrationHistoryService>();
            var installedMigration = migrationHistoryService.GetLast();

            SetCurrentCulture();
            AddDefaultMailSettings();
            InitMigration();
            InstallationStepsHelper.InheritCompositionForPage(
                CoreInstallationConstants.DocumentTypeAliases.Heading,
                NavigationInstallationConstants.DocumentTypeAliases.NavigationComposition);

            FixEmptyOwners();

            if (installedMigration != null)
            {
                new NotificationSettingsMigrations().Execute();
            }

            var uiNotificationMigration = DependencyResolver.Current.GetService<OldUiNotificationMigration>();
            uiNotificationMigration.Execute();

            DeleteExistedMailTemplates();

            new PagePromotionMigration().Execute();

            InstallationStepsHelper.InheritCompositionForPage(
                CoreInstallationConstants.DocumentTypeAliases.ContentPage,
                PagePromotionInstallationConstants.DocumentTypeAliases.PagePromotionComposition);

            new EventPublishDateMigration().Execute();
            new OldSubscribeSettingsMigration().Execute();
            new MoveMyGroupsOverviewDocTypeMigration().Execute();
            new TaggingMigration().Execute();
            new UpdateDataFolderNamesMigration().Execute();

            AddDefaultMailSettings();
            return Success;
        }

        public void Undo()
        {
            throw new NotImplementedException();
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
            umbracoContentMigration.UpdateActivitiesGrids();
            AddDefaultBackofficeSectionsToAdmin();
        }

        private void DeleteExistedMailTemplates()
        {
            var contentService = DependencyResolver.Current.GetService<IContentService>();
            var umbracoHelper = DependencyResolver.Current.GetService<UmbracoHelper>();
            var documentTypeAliasProvider = DependencyResolver.Current.GetService<IDocumentTypeAliasProvider>();

            var mailTemplateFolderXpath = XPathHelper.GetXpath(documentTypeAliasProvider.GetDataFolder(), documentTypeAliasProvider.GetMailTemplateFolder());
            var publishedContent = umbracoHelper.TypedContentSingleAtXPath(mailTemplateFolderXpath);

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