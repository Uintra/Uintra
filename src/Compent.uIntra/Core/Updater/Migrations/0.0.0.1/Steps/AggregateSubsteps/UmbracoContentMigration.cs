using System.Linq;
using System.Reflection;
using System.Web;
using Compent.Uintra.Core.Constants;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using EmailWorker.Data.Services.Interfaces;
using Uintra.Core;
using Uintra.Core.Extensions;
using Uintra.Core.Utils;
using Uintra.Navigation;
using Uintra.Notification.Configuration;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Steps.AggregateSubsteps
{
    public class UmbracoContentMigration
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IContentService _contentService;

        private readonly ISentMailsDocumentTypeService sentMailsDocumentTypeService;

        public UmbracoContentMigration()
        {
            _umbracoHelper = HttpContext.Current.GetService<UmbracoHelper>();
            sentMailsDocumentTypeService = HttpContext.Current.GetService<ISentMailsDocumentTypeService>();
            _contentService = ApplicationContext.Current.Services.ContentService;
        }

        public void Init()
        {
            CreateHomePage();

            CreateHomeNewsPages();
            CreateHomeEventsPages();
            CreateHomeBulletinsPages();

            CreateNotificationPage();
            CreateProfilePage();
            CreateProfileEditPage();
            CreateSearchResultPage();
            CreateErrorPage();

            CreateDataFolder();
            CreateGlobalPanelFolder();
            CreateSystemLinkFolder();

            CreateMailTemplatesFolderDataType();
            CreateMailWorkerDataTypes();
            CreateMailTemplatesFolder();

            CreateEventMailTemplate();
            CreateEventUpdatedMailTemplate();
            CreateEventHidedMailTemplate();
            CreateBeforeStartMailTemplate();
            CreateNewsMailTemplate();
            CreateActivityLikeAddedMailTemplate();
            CreateCommentAddedMailTemplate();
            CreateCommentEditedMailTemplate();
            CreateCommentRepliedMailTemplate();
            CreateCommentLikeAddedMailTemplate();

            CreateGroupOverviewPage();
            CreateGroupCreatePage();
            CreateGroupMyGroupsPage();
            CreateGroupsRoomPage();
            CreateGroupsSettingsPage();
            CreateGroupsMembersPage();
            CreateGroupsDeactivatedGroupPage();

            CreateGroupsNewsPages();
            CreateGroupsEventsPages();
            CreateGroupsBulletinsPages();
        }

        public void UpdateActivitiesGrids()
        {
            UpdateGrid(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage), "homePageGrid.json");
            UpdateGrid(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.NewsOverviewPage), "newsOverviewPageGrid.json");
            UpdateGrid(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.NewsOverviewPage, DocumentTypeAliasConstants.NewsCreatePage), "newsCreatePageGrid.json");
            UpdateGrid(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.NewsOverviewPage, DocumentTypeAliasConstants.NewsEditPage), "newsEditPageGrid.json");
            UpdateGrid(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.NewsOverviewPage, DocumentTypeAliasConstants.NewsDetailsPage), "newsDetailsPageGrid.json");

            UpdateGrid(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.EventsOverviewPage), "eventsOverviewPageGrid.json");
            UpdateGrid(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.EventsOverviewPage, DocumentTypeAliasConstants.EventsCreatePage), "eventsCreatePageGrid.json");
            UpdateGrid(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.EventsOverviewPage, DocumentTypeAliasConstants.EventsEditPage), "eventsEditPageGrid.json");
            UpdateGrid(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.EventsOverviewPage, DocumentTypeAliasConstants.EventsDetailsPage), "eventsDetailsPageGrid.json");

            UpdateGrid(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.BulletinsOverviewPage), "bulletinsOverviewPageGrid.json");
            UpdateGrid(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.BulletinsOverviewPage, DocumentTypeAliasConstants.BulletinsEditPage), "bulletinsEditPageGrid.json");
            UpdateGrid(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.BulletinsOverviewPage, DocumentTypeAliasConstants.BulletinsDetailsPage), "bulletinsDetailsPageGrid.json");
        }

        private void UpdateGrid(string pageXPath, string gridResourceFileName)
        {
            var page = _umbracoHelper.TypedContentSingleAtXPath(pageXPath);
            var pageContent = _contentService.GetById(page.Id);
            SetGridValueAndSaveAndPublishContent(pageContent, gridResourceFileName);
        }

        private void CreateHomePage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().FirstOrDefault(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.HomePage));
            if (homePage != null)
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("Home", -1, DocumentTypeAliasConstants.HomePage);
            content.SetValue(NavigationPropertiesConstants.NavigationNamePropName, "Home");

            SetGridValueAndSaveAndPublishContent(content, "homePageGrid.json");
        }

        private void CreateNotificationPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.HomePage));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.NotificationPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Notifications", homePage.Id, DocumentTypeAliasConstants.NotificationPage);
            content.SetValue("itemCountForPopup", 5);

            SetGridValueAndSaveAndPublishContent(content, "notificationPageGrid.json");
        }

        private void CreateProfilePage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.HomePage));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.ProfilePage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Profile", homePage.Id, DocumentTypeAliasConstants.ProfilePage);

            SetGridValueAndSaveAndPublishContent(content, "profilePageGrid.json");
        }

        private void CreateProfileEditPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.HomePage));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.ProfileEditPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Profile Edit", homePage.Id, DocumentTypeAliasConstants.ProfileEditPage);

            SetGridValueAndSaveAndPublishContent(content, "profileEditPageGrid.json");
        }

        private void CreateSearchResultPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.HomePage));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.SearchResultPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Search", homePage.Id, DocumentTypeAliasConstants.SearchResultPage);

            SetGridValueAndSaveAndPublishContent(content, "searchResultPageGrid.json");
        }

        private void CreateErrorPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.HomePage));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.ErrorPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Error", homePage.Id, DocumentTypeAliasConstants.ErrorPage);

            SetGridValueAndSaveAndPublishContent(content, "errorPageGrid.json");
        }

        private void CreateHomeNewsPages()
        {
            var homePageXpath = XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage);
            CreateNewsOverviewPage(homePageXpath, "newsOverviewPageGrid.json");
            var homeNewsOverviewPageXpath = XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.NewsOverviewPage);
            CreateNewsCreatePage(homeNewsOverviewPageXpath, "newsCreatePageGrid.json");
            CreateNewsEditPage(homeNewsOverviewPageXpath, "newsEditPageGrid.json");
            CreateNewsDetailsPage(homeNewsOverviewPageXpath, "newsDetailsPageGrid.json");
        }

        private void CreateGroupsNewsPages()
        {
            var groupsPageXpath = XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.GroupsOverviewPage, DocumentTypeAliasConstants.GroupsRoomPage);
            CreateNewsOverviewPage(groupsPageXpath, "groupsNewsOverviewPageGrid.json");
            var groupsNewsOverviewPageXpath = XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.GroupsOverviewPage, 
                DocumentTypeAliasConstants.GroupsRoomPage, DocumentTypeAliasConstants.NewsOverviewPage);
            CreateNewsCreatePage(groupsNewsOverviewPageXpath, "groupsNewsCreatePageGrid.json");
            CreateNewsEditPage(groupsNewsOverviewPageXpath, "groupsNewsEditPageGrid.json");
            CreateNewsDetailsPage(groupsNewsOverviewPageXpath, "groupsNewsDetailsPageGrid.json");
        }

        private void CreateNewsOverviewPage(string parentPageXpath, string gridResourceFileName)
        {
            var homePage = _umbracoHelper.TypedContentSingleAtXPath(parentPageXpath);
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.NewsOverviewPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("News", homePage.Id, DocumentTypeAliasConstants.NewsOverviewPage);

            SetGridValueAndSaveAndPublishContent(content, gridResourceFileName);
        }

        private void CreateNewsCreatePage(string parentPageXpath, string gridResourceFileName)
        {
            var newsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(parentPageXpath);
            if (newsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.NewsCreatePage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Create", newsOverviewPage.Id, DocumentTypeAliasConstants.NewsCreatePage);

            SetGridValueAndSaveAndPublishContent(content, gridResourceFileName);
        }

        private void CreateNewsEditPage(string parentPageXpath, string gridResourceFileName)
        {
            var newsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(parentPageXpath);
            if (newsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.NewsEditPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Edit", newsOverviewPage.Id, DocumentTypeAliasConstants.NewsEditPage);

            SetGridValueAndSaveAndPublishContent(content, gridResourceFileName);
        }

        private void CreateNewsDetailsPage(string parentPageXpath, string gridResourceFileName)
        {
            var newsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(parentPageXpath);
            if (newsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.NewsDetailsPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Details", newsOverviewPage.Id, DocumentTypeAliasConstants.NewsDetailsPage);

            SetGridValueAndSaveAndPublishContent(content, gridResourceFileName);
        }

        private void CreateHomeBulletinsPages()
        {
            var homePageXpath = XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage);
            CreateBulletinsOverviewPage(homePageXpath, "bulletinsOverviewPageGrid.json");
            var homeBulletinsOverviewPageXpath = XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.BulletinsOverviewPage);
            CreateBulletinsEditPage(homeBulletinsOverviewPageXpath, "bulletinsEditPageGrid.json");
            CreateBulletinsDetailsPage(homeBulletinsOverviewPageXpath, "bulletinsDetailsPageGrid.json");
        }

        private void CreateGroupsBulletinsPages()
        {
            var homePageXpath = XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.GroupsOverviewPage, DocumentTypeAliasConstants.GroupsRoomPage);
            CreateBulletinsOverviewPage(homePageXpath, "groupsBulletinsOverviewPageGrid.json");
            var homeBulletinsOverviewPageXpath = XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.GroupsOverviewPage,
                DocumentTypeAliasConstants.GroupsRoomPage, DocumentTypeAliasConstants.BulletinsOverviewPage);
            CreateBulletinsEditPage(homeBulletinsOverviewPageXpath, "groupsBulletinsEditPageGrid.json");
            CreateBulletinsDetailsPage(homeBulletinsOverviewPageXpath, "groupsBulletinsDetailsPageGrid.json");
        }

        private void CreateBulletinsOverviewPage(string parentPageXpath, string gridResourceFileName)
        {
            var homePage = _umbracoHelper.TypedContentSingleAtXPath(parentPageXpath);
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.BulletinsOverviewPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Bulletins", homePage.Id, DocumentTypeAliasConstants.BulletinsOverviewPage);

            SetGridValueAndSaveAndPublishContent(content, gridResourceFileName);
        }

        private void CreateBulletinsEditPage(string parentPageXpath, string gridResourceFileName)
        {
            var eventsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(parentPageXpath);
            if (eventsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.BulletinsEditPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Edit", eventsOverviewPage.Id, DocumentTypeAliasConstants.BulletinsEditPage);

            SetGridValueAndSaveAndPublishContent(content, gridResourceFileName);
        }

        private void CreateBulletinsDetailsPage(string parentPageXpath, string gridResourceFileName)
        {
            var eventsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(parentPageXpath);
            if (eventsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.BulletinsDetailsPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Details", eventsOverviewPage.Id, DocumentTypeAliasConstants.BulletinsDetailsPage);

            SetGridValueAndSaveAndPublishContent(content, gridResourceFileName);
        }

        private void CreateHomeEventsPages()
        {
            var homePageXpath = XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage);
            CreateEventsOverviewPage(homePageXpath, "eventsOverviewPageGrid.json");
            var homeEventsOverviewPageXpath = XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.EventsOverviewPage);
            CreateEventsCreatePage(homeEventsOverviewPageXpath, "eventsCreatePageGrid.json");
            CreateEventsEditPage(homeEventsOverviewPageXpath, "eventsEditPageGrid.json");
            CreateEventsDetailsPage(homeEventsOverviewPageXpath, "eventsDetailsPageGrid.json");
        }

        private void CreateGroupsEventsPages()
        {
            var homePageXpath = XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.GroupsOverviewPage, DocumentTypeAliasConstants.GroupsRoomPage);
            CreateEventsOverviewPage(homePageXpath, "groupsEventsOverviewPageGrid.json");
            var homeEventsOverviewPageXpath = XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.GroupsOverviewPage,
                DocumentTypeAliasConstants.GroupsRoomPage, DocumentTypeAliasConstants.EventsOverviewPage);
            CreateEventsCreatePage(homeEventsOverviewPageXpath, "groupsEventsCreatePageGrid.json");
            CreateEventsEditPage(homeEventsOverviewPageXpath, "groupsEventsEditPageGrid.json");
            CreateEventsDetailsPage(homeEventsOverviewPageXpath, "groupsEventsDetailsPageGrid.json");
        }

        private void CreateEventsOverviewPage(string parentPageXpath, string gridResourceFileName)
        {
            var homePage = _umbracoHelper.TypedContentSingleAtXPath(parentPageXpath);
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.EventsOverviewPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Events", homePage.Id, DocumentTypeAliasConstants.EventsOverviewPage);

            SetGridValueAndSaveAndPublishContent(content, gridResourceFileName);
        }

        private void CreateEventsCreatePage(string parentPageXpath, string gridResourceFileName)
        {
            var eventsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(parentPageXpath);
            if (eventsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.EventsCreatePage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Create", eventsOverviewPage.Id, DocumentTypeAliasConstants.EventsCreatePage);

            SetGridValueAndSaveAndPublishContent(content, gridResourceFileName);
        }

        private void CreateEventsEditPage(string parentPageXpath, string gridResourceFileName)
        {
            var eventsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(parentPageXpath);
            if (eventsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.EventsEditPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Edit", eventsOverviewPage.Id, DocumentTypeAliasConstants.EventsEditPage);

            SetGridValueAndSaveAndPublishContent(content, gridResourceFileName);
        }

        private void CreateEventsDetailsPage(string parentPageXpath, string gridResourceFileName)
        {
            var eventsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(parentPageXpath);
            if (eventsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.EventsDetailsPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Details", eventsOverviewPage.Id, DocumentTypeAliasConstants.EventsDetailsPage);

            SetGridValueAndSaveAndPublishContent(content, gridResourceFileName);
        }

        private void CreateDataFolder()
        {
            var dataFolder = _umbracoHelper.TypedContentAtRoot().FirstOrDefault(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.DataFolder));
            if (dataFolder != null)
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity(CoreInstallationConstants.ContentDefaultName.DataFolder, -1, DocumentTypeAliasConstants.DataFolder);

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateGlobalPanelFolder()
        {
            var dataFolder = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.DataFolder));
            if (dataFolder.Children.Any(el => el.DocumentTypeAlias.Equals(PanelsInstallationConstants.DocumentTypeAliases.GlobalPanelFolder)))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity(PanelsInstallationConstants.ContentDefaultName.GlobalPanelFolder, dataFolder.Id, PanelsInstallationConstants.DocumentTypeAliases.GlobalPanelFolder);

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateSystemLinkFolder()
        {
            var dataFolder = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.DataFolder));
            if (dataFolder.Children.Any(el => el.DocumentTypeAlias.Equals(NavigationInstallationConstants.DocumentTypeAliases.SystemLinkFolder)))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity(NavigationInstallationConstants.ContentDefaultName.SystemLinkFolder, dataFolder.Id, NavigationInstallationConstants.DocumentTypeAliases.SystemLinkFolder);

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateMailWorkerDataTypes()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;

            var mailTemplateDocType = contentService.GetContentType(DocumentTypeAliasConstants.MailTemplate);
            if (mailTemplateDocType != null) return;

            var dataContentFolder = contentService.GetContentTypeContainers(CoreInstallationConstants.DocumentTypesContainerNames.DataContent, 1).First();
            sentMailsDocumentTypeService.CreateMailTemplateDocTypes(dataContentFolder.Id.ToString());

            mailTemplateDocType = contentService.GetContentType(DocumentTypeAliasConstants.MailTemplate);

            mailTemplateDocType.RemovePropertyType(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName);
            var notificationTypeEnumDropdown = dataTypeService.GetDataTypeDefinitionByName(NotificationInstallationConstants.DataTypeNames.NotificationTypeEnum);
            var newEmailTypeProperty = new PropertyType(notificationTypeEnumDropdown)
            {
                Name = "Email type",
                Alias = "emailType"
            };

            mailTemplateDocType.AddPropertyType(newEmailTypeProperty, "Content");
            contentService.Save(mailTemplateDocType);

            InstallationStepsHelper.AddAllowedChildNode(DocumentTypeAliasConstants.MailTemplatesFolder, DocumentTypeAliasConstants.MailTemplate);
        }
        private void CreateMailTemplatesFolderDataType()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;
            var mailTemplateFolderDataType = contentService.GetContentType(DocumentTypeAliasConstants.MailTemplatesFolder);
            if (mailTemplateFolderDataType != null) return;

            var dataContentFolder = contentService.GetContentTypeContainers(CoreInstallationConstants.DocumentTypesContainerNames.Folders, 1).First();

            mailTemplateFolderDataType = new ContentType(dataContentFolder.Id)
            {
                Name = UmbracoContentMigrationConstants.MailTemplate.MailTemplatesFolderName,
                Alias = DocumentTypeAliasConstants.MailTemplatesFolder
            };

            contentService.Save(mailTemplateFolderDataType);

        }
        private void CreateMailTemplatesFolder()
        {
            var dataFolder = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.DataFolder));
            if (dataFolder.Children.Any(el => el.DocumentTypeAlias.Equals(MailInstallationConstants.DocumentTypeAliases.MailTemplatesFolder)))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity(MailInstallationConstants.ContentDefaultName.MailTemplatesFolder, dataFolder.Id, MailInstallationConstants.DocumentTypeAliases.MailTemplatesFolder);

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateEventMailTemplate()
        {
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.DataFolder, DocumentTypeAliasConstants.MailTemplatesFolder));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<LegacyNotificationTypes>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == LegacyNotificationTypes.Event))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("Event", mailTemplatesFolder.Id, DocumentTypeAliasConstants.MailTemplate);
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.SubjectPropName, "Event");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.BodyPropName, "<p>Event</p><p>FullName: ##FullName##</p>");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.ExtraTokensPropName, "##FullName##");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName, LegacyNotificationTypes.Event.ToString());

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateEventUpdatedMailTemplate()
        {
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.DataFolder, DocumentTypeAliasConstants.MailTemplatesFolder));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.EventUpdated))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("EventUpdated", mailTemplatesFolder.Id, DocumentTypeAliasConstants.MailTemplate);
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.SubjectPropName, "Dear ##FullName## the event was updated");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.BodyPropName, @"<p>EventUpdated</p>
<p>FullName: ##FullName##</p>
<p> </p>
<p><span>Title: </span>##Title##</p>
<p><span>Url: <a rel=""noopener noreferrer"" href=""##Url##"" target=""_blank"">link</a></span></p>
<p><span>Type: </span>##Type##</p>");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.ExtraTokensPropName, "##Title##, ##Url##, ##Type##, ##FullName##");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName, NotificationTypeEnum.EventUpdated.ToString());

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateEventHidedMailTemplate()
        {
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.DataFolder, DocumentTypeAliasConstants.MailTemplatesFolder));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.EventHidden))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("EventHided", mailTemplatesFolder.Id, DocumentTypeAliasConstants.MailTemplate);
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.SubjectPropName, "EventHided");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.BodyPropName, @"<p>EventHided</p>
<p><span>FullName: </span>##FullName##</p>
<p> </p>
<p><span>Title: </span>##Title##</p>
<p><span>NotifierFullName: </span>##NotifierFullName##</p>
<p><span>Type: </span>##Type##</p>
<p><span>Url: </span><a rel=""noopener noreferrer"" href=""##Url##"" target=""_blank"">link</a></p>");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.ExtraTokensPropName, "##Title##, ##NotifierFullName##, ##Type##, ##Url##, ##FullName##");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName, NotificationTypeEnum.EventHidden.ToString());

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateBeforeStartMailTemplate()
        {
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.DataFolder, DocumentTypeAliasConstants.MailTemplatesFolder));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.BeforeStart))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("BeforeStart", mailTemplatesFolder.Id, DocumentTypeAliasConstants.MailTemplate);
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.SubjectPropName, "BeforeStart");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.BodyPropName, @"<p>BeforeStart</p>
<p><span>FullName: </span><span>##FullName##</span></p>
<p> </p>
<p><span>ActivityTitle: </span>##ActivityTitle##</p>
<p><span>ActivityType: </span>##ActivityType##</p>
<p><span>StartDate: </span>##StartDate##</p>
<p><span>Url: </span><a rel=""noopener noreferrer"" href=""##Url##"" target=""_blank"">link</a></p>");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.ExtraTokensPropName, "##ActivityTitle##,##ActivityType##,##StartDate##,##Url##, ##FullName##");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName, NotificationTypeEnum.BeforeStart.ToString());

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateNewsMailTemplate()
        {
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.DataFolder, DocumentTypeAliasConstants.MailTemplatesFolder));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<LegacyNotificationTypes>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == LegacyNotificationTypes.News))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("News", mailTemplatesFolder.Id, DocumentTypeAliasConstants.MailTemplate);
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.SubjectPropName, "News");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.BodyPropName, @"<p>News</p>
<p>FullName: ##FullName##</p>");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.ExtraTokensPropName, "##FullName##");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName, LegacyNotificationTypes.News.ToString());

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateActivityLikeAddedMailTemplate()
        {
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.DataFolder, DocumentTypeAliasConstants.MailTemplatesFolder));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.ActivityLikeAdded))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("ActivityLikeAdded", mailTemplatesFolder.Id, DocumentTypeAliasConstants.MailTemplate);
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.SubjectPropName, "ActivityLikeAdded");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.BodyPropName, @"<p>ActivityLikeAdded</p>
<p>FullName: ##FullName##</p>
<p> </p>
<p><span>ActivityTitle: </span>##ActivityTitle##</p>
<p><span>ActivityType: </span>##ActivityType##</p>
<p><span>CreatedDate: </span>##CreatedDate##</p>
<p><span>Url: </span><a rel=""noopener noreferrer"" href=""##Url##"" target=""_blank"">link</a></p>");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.ExtraTokensPropName, "##ActivityTitle##, ##Url##, ##ActivityType##, ##CreatedDate##, ##FullName##");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName, NotificationTypeEnum.ActivityLikeAdded.ToString());

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateCommentAddedMailTemplate()
        {
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.DataFolder, DocumentTypeAliasConstants.MailTemplatesFolder));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.CommentAdded))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("CommentAdded", mailTemplatesFolder.Id, DocumentTypeAliasConstants.MailTemplate);
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.SubjectPropName, "CommentAdded");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.BodyPropName, @"<p>CommentAdded</p>
<p>FullName: ##FullName##</p>
<p> </p>
<p><span>ActivityTitle: </span>##ActivityTitle##</p>
<p><span>Url: </span><a rel=""noopener noreferrer"" href=""##Url##"" target=""_blank"">link</a></p>");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.ExtraTokensPropName, "##ActivityTitle##, ##Url##, ##FullName##");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName, NotificationTypeEnum.CommentAdded.ToString());

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateCommentEditedMailTemplate()
        {
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.DataFolder, DocumentTypeAliasConstants.MailTemplatesFolder));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.CommentEdited))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("CommentEdited", mailTemplatesFolder.Id, DocumentTypeAliasConstants.MailTemplate);
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.SubjectPropName, "CommentEdited");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.BodyPropName, @"<p>CommentEdited</p>
<p>FullName: ##FullName##</p>
<p> </p>
<p><span>ActivityTitle: </span>##ActivityTitle##</p>
<p><span>Url: </span><a rel=""noopener noreferrer"" href=""##Url##"" target=""_blank"">link</a></p>");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.ExtraTokensPropName, "##ActivityTitle##, ##Url##, ##FullName##");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName, NotificationTypeEnum.CommentEdited.ToString());

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateCommentRepliedMailTemplate()
        {
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.DataFolder, DocumentTypeAliasConstants.MailTemplatesFolder));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.CommentReplied))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("CommentReplied", mailTemplatesFolder.Id, DocumentTypeAliasConstants.MailTemplate);
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.SubjectPropName, "CommentReplied");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.BodyPropName, @"<p>CommentReplyed</p>
<p>FullName: ##FullName##</p>
<p> </p>
<p>ActivityTitle: ##ActivityTitle##</p>
<p><span>Url: </span><a rel=""noopener noreferrer"" href=""##Url##"" target=""_blank"">link</a></p>");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.ExtraTokensPropName, "##ActivityTitle##, ##Url##, ##FullName##");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName, NotificationTypeEnum.CommentReplied.ToString());

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateCommentLikeAddedMailTemplate()
        {
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.DataFolder, DocumentTypeAliasConstants.MailTemplatesFolder));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.CommentLikeAdded))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("CommentLikeAdded", mailTemplatesFolder.Id, DocumentTypeAliasConstants.MailTemplate);
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.SubjectPropName, "CommentLikeAdded");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.BodyPropName, @"<p>CommentLikeAdded</p>
<p>FullName: ##FullName##</p>");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.ExtraTokensPropName, "##FullName##");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName, NotificationTypeEnum.CommentLikeAdded.ToString());

            _contentService.SaveAndPublishWithStatus(content);
        }

        public void SetGridValueAndSaveAndPublishContent(IContent content, string gridEmbeddedResourceFileName)
        {
            var gridContent = EmbeddedResourcesUtils.ReadResourceContent($"{Assembly.GetExecutingAssembly().GetName().Name}.Core.Updater.Migrations._0._0._0._1.PreValues.ContentPageJsons.{gridEmbeddedResourceFileName}");
            content.SetValue(UmbracoContentMigrationConstants.Grid.GridPropName, gridContent);

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateGroupOverviewPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.HomePage));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.GroupsOverviewPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Groups", homePage.Id, DocumentTypeAliasConstants.GroupsOverviewPage);

            SetGridValueAndSaveAndPublishContent(content, "groupsOverviewPageGrid.json");
        }

        private void CreateGroupCreatePage()
        {
            var newsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.GroupsOverviewPage));
            if (newsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.GroupsCreatePage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Create", newsOverviewPage.Id, DocumentTypeAliasConstants.GroupsCreatePage);
            content.SetValue(NavigationPropertiesConstants.NavigationNamePropName, "Create");
            content.SetValue(NavigationPropertiesConstants.IsHideFromLeftNavigationPropName, false);
            content.SetValue(NavigationPropertiesConstants.IsHideFromSubNavigationPropName, false);

            SetGridValueAndSaveAndPublishContent(content, "groupsCreatePageGrid.json");
        }

        private void CreateGroupMyGroupsPage()
        {
            var newsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.GroupsOverviewPage));
            if (newsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.GroupsMyGroupsOverviewPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("My Groups", newsOverviewPage.Id, DocumentTypeAliasConstants.GroupsMyGroupsOverviewPage);
            content.SetValue(NavigationPropertiesConstants.NavigationNamePropName, "My Groups");
            content.SetValue(NavigationPropertiesConstants.IsHideFromLeftNavigationPropName, false);
            content.SetValue(NavigationPropertiesConstants.IsHideFromSubNavigationPropName, false);

            SetGridValueAndSaveAndPublishContent(content, "groupsMyGroupsOverviewPageGrid.json");
        }

        private void CreateGroupsRoomPage()
        {
            var newsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.GroupsOverviewPage));
            if (newsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.GroupsRoomPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Room", newsOverviewPage.Id, DocumentTypeAliasConstants.GroupsRoomPage);
            content.SetValue(NavigationPropertiesConstants.NavigationNamePropName, "Room");
            content.SetValue(NavigationPropertiesConstants.IsHideFromLeftNavigationPropName, true);
            content.SetValue(NavigationPropertiesConstants.IsHideFromSubNavigationPropName, true);

            SetGridValueAndSaveAndPublishContent(content, "groupsRoomPageGrid.json");
        }

        private void CreateGroupsSettingsPage()
        {
            var newsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.GroupsOverviewPage, DocumentTypeAliasConstants.GroupsRoomPage));
            if (newsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.GroupsEditPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Settings", newsOverviewPage.Id, DocumentTypeAliasConstants.GroupsEditPage);
            content.SetValue(NavigationPropertiesConstants.NavigationNamePropName, "Settings");
            content.SetValue(NavigationPropertiesConstants.IsHideFromLeftNavigationPropName, true);
            content.SetValue(NavigationPropertiesConstants.IsHideFromSubNavigationPropName, false);

            SetGridValueAndSaveAndPublishContent(content, "groupsEditPageGrid.json");
        }

        private void CreateGroupsMembersPage()
        {
            var newsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.GroupsOverviewPage, DocumentTypeAliasConstants.GroupsRoomPage));
            if (newsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.GroupsMembersPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Group Members", newsOverviewPage.Id, DocumentTypeAliasConstants.GroupsMembersPage);
            content.SetValue(NavigationPropertiesConstants.NavigationNamePropName, "Group Members");
            content.SetValue(NavigationPropertiesConstants.IsHideFromLeftNavigationPropName, true);
            content.SetValue(NavigationPropertiesConstants.IsHideFromSubNavigationPropName, false);

            SetGridValueAndSaveAndPublishContent(content, "groupsMembersPageGrid.json");
        }

        private void CreateGroupsDeactivatedGroupPage()
        {
            var newsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.GroupsOverviewPage, DocumentTypeAliasConstants.GroupsRoomPage));
            if (newsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.GroupsDeactivatedGroupPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Disabled", newsOverviewPage.Id, DocumentTypeAliasConstants.GroupsDeactivatedGroupPage);
            content.SetValue(NavigationPropertiesConstants.NavigationNamePropName, "Disabled");
            content.SetValue(NavigationPropertiesConstants.IsHideFromLeftNavigationPropName, true);
            content.SetValue(NavigationPropertiesConstants.IsHideFromSubNavigationPropName, false);

            SetGridValueAndSaveAndPublishContent(content, "groupsDeactivatedGroupPageGrid.json");
        }
    }

    public static class UmbracoContentMigrationConstants
    {
        public static class Grid
        {
            public const string GridPropName = "grid";
        }

        public static class MailTemplate
        {
            public const string SubjectPropName = "subject";
            public const string BodyPropName = "body";
            public const string ExtraTokensPropName = "extraTokens";
            public const string EmailTypePropName = "emailType";
            public const string MailTemplatesFolderName = "Mail Templates Folder";
        }
    }
}