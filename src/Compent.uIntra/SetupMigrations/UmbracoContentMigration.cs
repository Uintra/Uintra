using System;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using EmailWorker.Data.Services.Interfaces;
using uIntra.Core;
using uIntra.Core.Extentions;
using uIntra.Core.Installer;
using uIntra.Notification.Configuration;
using uIntra.Notification.Installer;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using File = System.IO.File;

namespace Compent.uIntra.SetupMigrations
{
    public class UmbracoContentMigration
    {
        private const string JsonFilesFolder = "~/SetupMigrations/json/";

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

            CreateNewsOverviewPage();
            CreateNewsCreatePage();
            CreateNewsEditPage();
            CreateNewsDetailsPage();

            CreateEventsOverviewPage();
            CreateEventsCreatePage();
            CreateEventsEditPage();
            CreateEventsDetailsPage();

            CreateBulletinsOverviewPage();
            CreateBulletinsDetailsPage();
            CreateBulletinsEditPage();

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
        }

        private void CreateHomePage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().FirstOrDefault(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName));
            if (homePage != null)
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("Home Page", -1, UmbracoContentMigrationConstants.DocType.HomePageDocTypeName);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.NavigationNamePropName, "Home Page");

            SetGridValueAndSaveAndPublishContent(content, "homePageGrid.json");
        }

        private void CreateNotificationPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.NotificationPageDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContent("Notifications", homePage.Id, UmbracoContentMigrationConstants.DocType.NotificationPageDocTypeName);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.NavigationNamePropName, "Notifications");
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromLeftNavigationPropName, true);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromSubNavigationPropName, true);
            content.SetValue("itemCountForPopup", 5);

            SetGridValueAndSaveAndPublishContent(content, "notificationPageGrid.json");
        }

        private void CreateProfilePage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.ProfilePageDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContent("Profile", homePage.Id, UmbracoContentMigrationConstants.DocType.ProfilePageDocTypeName);
            //content.SetValue(UmbracoContentMigrationConstants.Navigation.NavigationNamePropName, "Profile");
            //content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromLeftNavigationPropName, true);
            //content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromSubNavigationPropName, true);

            SetGridValueAndSaveAndPublishContent(content, "profilePageGrid.json");
        }

        private void CreateProfileEditPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.ProfileEditPageDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContent("Profile Edit Page", homePage.Id, UmbracoContentMigrationConstants.DocType.ProfileEditPageDocTypeName);
            //content.SetValue(UmbracoContentMigrationConstants.Navigation.NavigationNamePropName, "Profile Edit");
            //content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromLeftNavigationPropName, true);
            //content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromSubNavigationPropName, true);

            SetGridValueAndSaveAndPublishContent(content, "profileEditPageGrid.json");
        }

        private void CreateSearchResultPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.SearchResultPageDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContent("Search Page", homePage.Id, UmbracoContentMigrationConstants.DocType.SearchResultPageDocTypeName);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.NavigationNamePropName, "Search Page");
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromLeftNavigationPropName, true);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromSubNavigationPropName, true);

            SetGridValueAndSaveAndPublishContent(content, "searchResultPageGrid.json");
        }

        private void CreateErrorPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.ErrorPageDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContent("Error Page", homePage.Id, UmbracoContentMigrationConstants.DocType.ErrorPageDocTypeName);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.NavigationNamePropName, "Error Page");
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromLeftNavigationPropName, true);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromSubNavigationPropName, true);

            SetGridValueAndSaveAndPublishContent(content, "errorPageGrid.json");
        }

        private void CreateNewsOverviewPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.NewsOverviewPageDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContent("News", homePage.Id, UmbracoContentMigrationConstants.DocType.NewsOverviewPageDocTypeName);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.NavigationNamePropName, "News");
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromLeftNavigationPropName, false);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromSubNavigationPropName, false);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsShowInHomeNavigationPropName, true);

            SetGridValueAndSaveAndPublishContent(content, "newsOverviewPageGrid.json");
        }

        private void CreateNewsCreatePage()
        {
            var newsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName, UmbracoContentMigrationConstants.DocType.NewsOverviewPageDocTypeName));
            if (newsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.NewsCreatePageDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContent("Create", newsOverviewPage.Id, UmbracoContentMigrationConstants.DocType.NewsCreatePageDocTypeName);

            SetGridValueAndSaveAndPublishContent(content, "newsCreatePageGrid.json");
        }

        private void CreateNewsEditPage()
        {
            var newsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName, UmbracoContentMigrationConstants.DocType.NewsOverviewPageDocTypeName));
            if (newsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.NewsEditPageDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContent("Edit", newsOverviewPage.Id, UmbracoContentMigrationConstants.DocType.NewsEditPageDocTypeName);

            SetGridValueAndSaveAndPublishContent(content, "newsEditPageGrid.json");
        }

        private void CreateNewsDetailsPage()
        {
            var newsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName, UmbracoContentMigrationConstants.DocType.NewsOverviewPageDocTypeName));
            if (newsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.NewsDetailsPageDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContent("Details", newsOverviewPage.Id, UmbracoContentMigrationConstants.DocType.NewsDetailsPageDocTypeName);

            SetGridValueAndSaveAndPublishContent(content, "newsDetailsPageGrid.json");
        }

        private void CreateBulletinsOverviewPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.BulletinsOverviewPageDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContent("Bulletins", homePage.Id, UmbracoContentMigrationConstants.DocType.BulletinsOverviewPageDocTypeName);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.NavigationNamePropName, "Bulletins");
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromLeftNavigationPropName, false);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromSubNavigationPropName, false);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsShowInHomeNavigationPropName, true);

            SetGridValueAndSaveAndPublishContent(content, "bulletinsOverviewPageGrid.json");
        }

        private void CreateBulletinsEditPage()
        {
            var eventsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName, UmbracoContentMigrationConstants.DocType.BulletinsOverviewPageDocTypeName));
            if (eventsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.BulletinsEditPageDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContent("Edit", eventsOverviewPage.Id, UmbracoContentMigrationConstants.DocType.BulletinsEditPageDocTypeName);

            SetGridValueAndSaveAndPublishContent(content, "bulletinsEditPageGrid.json");
        }

        private void CreateBulletinsDetailsPage()
        {
            var eventsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName, UmbracoContentMigrationConstants.DocType.BulletinsOverviewPageDocTypeName));
            if (eventsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.BulletinsDetailsPageDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContent("Details", eventsOverviewPage.Id, UmbracoContentMigrationConstants.DocType.BulletinsDetailsPageDocTypeName);
            SetGridValueAndSaveAndPublishContent(content, "bulletinsDetailsPageGrid.json");
        }

        private void CreateEventsOverviewPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.EventsOverviewPageDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContent("Events", homePage.Id, UmbracoContentMigrationConstants.DocType.EventsOverviewPageDocTypeName);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.NavigationNamePropName, "Events");
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromLeftNavigationPropName, false);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromSubNavigationPropName, false);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsShowInHomeNavigationPropName, true);

            SetGridValueAndSaveAndPublishContent(content, "eventsOverviewPageGrid.json");
        }

        private void CreateEventsCreatePage()
        {
            var eventsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName, UmbracoContentMigrationConstants.DocType.EventsOverviewPageDocTypeName));
            if (eventsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.EventsCreatePageDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContent("Create", eventsOverviewPage.Id, UmbracoContentMigrationConstants.DocType.EventsCreatePageDocTypeName);

            SetGridValueAndSaveAndPublishContent(content, "eventsCreatePageGrid.json");
        }

        private void CreateEventsEditPage()
        {
            var eventsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName, UmbracoContentMigrationConstants.DocType.EventsOverviewPageDocTypeName));
            if (eventsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.EventsEditPageDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContent("Edit", eventsOverviewPage.Id, UmbracoContentMigrationConstants.DocType.EventsEditPageDocTypeName);

            SetGridValueAndSaveAndPublishContent(content, "eventsEditPageGrid.json");
        }

        private void CreateEventsDetailsPage()
        {
            var eventsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName, UmbracoContentMigrationConstants.DocType.EventsOverviewPageDocTypeName));
            if (eventsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.EventsDetailsPageDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContent("Details", eventsOverviewPage.Id, UmbracoContentMigrationConstants.DocType.EventsDetailsPageDocTypeName);
            SetGridValueAndSaveAndPublishContent(content, "eventsDetailsPageGrid.json");
        }

        private void CreateDataFolder()
        {
            var dataFolder = _umbracoHelper.TypedContentAtRoot().FirstOrDefault(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.DataFolderDocTypeName));
            if (dataFolder != null)
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("Data folder", -1, UmbracoContentMigrationConstants.DocType.DataFolderDocTypeName);

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateGlobalPanelFolder()
        {
            var dataFolder = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.DataFolderDocTypeName));
            if (dataFolder.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.GlobalPanelFolderDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("Global Panel Folder", dataFolder.Id, UmbracoContentMigrationConstants.DocType.GlobalPanelFolderDocTypeName);

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateSystemLinkFolder()
        {
            var dataFolder = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.DataFolderDocTypeName));
            if (dataFolder.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.SystemLinkFolderDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("System Link Folder", dataFolder.Id, UmbracoContentMigrationConstants.DocType.SystemLinkFolderDocTypeName);

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateMailWorkerDataTypes()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;

            var mailTemplateDocType = contentService.GetContentType(UmbracoContentMigrationConstants.DocType.MailTemplateDocTypeName);
            if (mailTemplateDocType != null) return;

            var dataContentFolder = contentService.GetContentTypeContainers(CoreInstallationConstants.DocumentTypesContainerNames.DataContent, 1).First();
            sentMailsDocumentTypeService.CreateMailTemplateDocTypes(dataContentFolder.Id.ToString());

            mailTemplateDocType = contentService.GetContentType(UmbracoContentMigrationConstants.DocType.MailTemplateDocTypeName);

            mailTemplateDocType.RemovePropertyType(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName);
            var notificationTypeEnumDropdown = dataTypeService.GetDataTypeDefinitionByName(NotificationInstallationConstants.DataTypeNames.NotificationTypeEnum);
            var newEmailTypeProperty = new PropertyType(notificationTypeEnumDropdown)
            {
                Name = "Email type",
                Alias = "emailType"
            };

            mailTemplateDocType.AddPropertyType(newEmailTypeProperty, "Content");
            contentService.Save(mailTemplateDocType);

            CoreInstallationStep.AddAllowedChildNode(UmbracoContentMigrationConstants.DocType.MailTemplatesFolderDocTypeName, UmbracoContentMigrationConstants.DocType.MailTemplateDocTypeName);
        }
        private void CreateMailTemplatesFolderDataType()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;
            var mailTemplateFolderDataType = contentService.GetContentType(UmbracoContentMigrationConstants.DocType.MailTemplatesFolderDocTypeName);
            if (mailTemplateFolderDataType != null) return;

            var dataContentFolder = contentService.GetContentTypeContainers(CoreInstallationConstants.DocumentTypesContainerNames.Folders, 1).First();

            mailTemplateFolderDataType = new ContentType(dataContentFolder.Id)
            {
                Name = UmbracoContentMigrationConstants.MailTemplate.MailTemplatesFolderName,
                Alias = UmbracoContentMigrationConstants.DocType.MailTemplatesFolderDocTypeName
            };

            contentService.Save(mailTemplateFolderDataType);

        }
        private void CreateMailTemplatesFolder()
        {
            var dataFolder = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.DataFolderDocTypeName));
            if (dataFolder.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.MailTemplatesFolderDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("Mail Templates Folder", dataFolder.Id, UmbracoContentMigrationConstants.DocType.MailTemplatesFolderDocTypeName);

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateEventMailTemplate()
        {
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(UmbracoContentMigrationConstants.DocType.DataFolderDocTypeName, UmbracoContentMigrationConstants.DocType.MailTemplatesFolderDocTypeName));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.Event))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("Event", mailTemplatesFolder.Id, UmbracoContentMigrationConstants.DocType.MailTemplateDocTypeName);
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.SubjectPropName, "Event");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.BodyPropName, "<p>Event</p><p>FullName: ##FullName##</p>");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.ExtraTokensPropName, "##FullName##");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName, NotificationTypeEnum.Event.ToString());

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateEventUpdatedMailTemplate()
        {
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(UmbracoContentMigrationConstants.DocType.DataFolderDocTypeName, UmbracoContentMigrationConstants.DocType.MailTemplatesFolderDocTypeName));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.EventUpdated))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("EventUpdated", mailTemplatesFolder.Id, UmbracoContentMigrationConstants.DocType.MailTemplateDocTypeName);
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
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(UmbracoContentMigrationConstants.DocType.DataFolderDocTypeName, UmbracoContentMigrationConstants.DocType.MailTemplatesFolderDocTypeName));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.EventHided))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("EventHided", mailTemplatesFolder.Id, UmbracoContentMigrationConstants.DocType.MailTemplateDocTypeName);
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.SubjectPropName, "EventHided");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.BodyPropName, @"<p>EventHided</p>
<p><span>FullName: </span>##FullName##</p>
<p> </p>
<p><span>Title: </span>##Title##</p>
<p><span>NotifierFullName: </span>##NotifierFullName##</p>
<p><span>Type: </span>##Type##</p>
<p><span>Url: </span><a rel=""noopener noreferrer"" href=""##Url##"" target=""_blank"">link</a></p>");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.ExtraTokensPropName, "##Title##, ##NotifierFullName##, ##Type##, ##Url##, ##FullName##");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName, NotificationTypeEnum.EventHided.ToString());

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateBeforeStartMailTemplate()
        {
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(UmbracoContentMigrationConstants.DocType.DataFolderDocTypeName, UmbracoContentMigrationConstants.DocType.MailTemplatesFolderDocTypeName));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.BeforeStart))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("BeforeStart", mailTemplatesFolder.Id, UmbracoContentMigrationConstants.DocType.MailTemplateDocTypeName);
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
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(UmbracoContentMigrationConstants.DocType.DataFolderDocTypeName, UmbracoContentMigrationConstants.DocType.MailTemplatesFolderDocTypeName));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.News))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("News", mailTemplatesFolder.Id, UmbracoContentMigrationConstants.DocType.MailTemplateDocTypeName);
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.SubjectPropName, "News");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.BodyPropName, @"<p>News</p>
<p>FullName: ##FullName##</p>");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.ExtraTokensPropName, "##FullName##");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName, NotificationTypeEnum.News.ToString());

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateActivityLikeAddedMailTemplate()
        {
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(UmbracoContentMigrationConstants.DocType.DataFolderDocTypeName, UmbracoContentMigrationConstants.DocType.MailTemplatesFolderDocTypeName));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.ActivityLikeAdded))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("ActivityLikeAdded", mailTemplatesFolder.Id, UmbracoContentMigrationConstants.DocType.MailTemplateDocTypeName);
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
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(UmbracoContentMigrationConstants.DocType.DataFolderDocTypeName, UmbracoContentMigrationConstants.DocType.MailTemplatesFolderDocTypeName));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.CommentAdded))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("CommentAdded", mailTemplatesFolder.Id, UmbracoContentMigrationConstants.DocType.MailTemplateDocTypeName);
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
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(UmbracoContentMigrationConstants.DocType.DataFolderDocTypeName, UmbracoContentMigrationConstants.DocType.MailTemplatesFolderDocTypeName));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.CommentEdited))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("CommentEdited", mailTemplatesFolder.Id, UmbracoContentMigrationConstants.DocType.MailTemplateDocTypeName);
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
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(UmbracoContentMigrationConstants.DocType.DataFolderDocTypeName, UmbracoContentMigrationConstants.DocType.MailTemplatesFolderDocTypeName));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.CommentReplied))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("CommentReplied", mailTemplatesFolder.Id, UmbracoContentMigrationConstants.DocType.MailTemplateDocTypeName);
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
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(UmbracoContentMigrationConstants.DocType.DataFolderDocTypeName, UmbracoContentMigrationConstants.DocType.MailTemplatesFolderDocTypeName));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.CommentLikeAdded))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("CommentLikeAdded", mailTemplatesFolder.Id, UmbracoContentMigrationConstants.DocType.MailTemplateDocTypeName);
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.SubjectPropName, "CommentLikeAdded");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.BodyPropName, @"<p>CommentLikeAdded</p>
<p>FullName: ##FullName##</p>");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.ExtraTokensPropName, "##FullName##");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName, NotificationTypeEnum.CommentLikeAdded.ToString());

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void SetGridValueAndSaveAndPublishContent(IContent content, string gridfileName)
        {
            var gridContent = GetGridContent(gridfileName);
            content.SetValue(UmbracoContentMigrationConstants.Grid.GridPropName, gridContent);

            _contentService.SaveAndPublishWithStatus(content);
        }

        private static string GetGridContent(string fileName)
        {
            var filePath = HostingEnvironment.MapPath($"{JsonFilesFolder}{fileName}");
            if (filePath.IsNullOrEmpty() || !File.Exists(filePath))
            {
                throw new Exception($"File {fileName} doesn't exist.");
            }

            return File.ReadAllText(filePath);
        }
    }

    public static class UmbracoContentMigrationConstants
    {
        public static class DocType
        {
            public const string HomePageDocTypeName = "homePage";
            public const string NotificationPageDocTypeName = "notificationPage";
            public const string ProfilePageDocTypeName = "profilePage";
            public const string ProfileEditPageDocTypeName = "profileEditPage";
            public const string SearchResultPageDocTypeName = "searchResultPage";

            public const string NewsOverviewPageDocTypeName = "newsOverviewPage";
            public const string NewsCreatePageDocTypeName = "newsCreatePage";
            public const string NewsEditPageDocTypeName = "newsEditPage";
            public const string NewsDetailsPageDocTypeName = "newsDetailsPage";

            public const string EventsOverviewPageDocTypeName = "eventsOverviewPage";
            public const string EventsCreatePageDocTypeName = "eventsCreatePage";
            public const string EventsEditPageDocTypeName = "eventsEditPage";
            public const string EventsDetailsPageDocTypeName = "eventsDetailsPage";

            public const string BulletinsOverviewPageDocTypeName = "bulletinsOverviewPage";
            public const string BulletinsEditPageDocTypeName = "bulletinsEditPage";
            public const string BulletinsDetailsPageDocTypeName = "bulletinsDetailsPage";

            public const string DataFolderDocTypeName = "dataFolder";
            public const string GlobalPanelFolderDocTypeName = "globalPanelFolder";
            public const string SystemLinkFolderDocTypeName = "systemLinkFolder";
            public const string MailTemplatesFolderDocTypeName = "mailTemplatesFolder";
            public const string MailTemplateDocTypeName = "MailTemplate";
            public const string ErrorPageDocTypeName = "errorPage";
        }

        public static class Navigation
        {
            public const string NavigationNamePropName = "navigationName";
            public const string IsHideFromLeftNavigationPropName = "isHideFromLeftNavigation";
            public const string IsHideFromSubNavigationPropName = "isHideFromSubNavigation";
            public const string IsShowInHomeNavigationPropName = "isShowInHomeNavigation";
        }

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