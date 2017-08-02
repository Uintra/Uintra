using System;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Compent.uIntra.Core.Constants;
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

namespace Compent.uIntra.Installer
{
    public class UmbracoContentMigration
    {
        private const string JsonFilesFolder = "~/Installer/ContentPageJsons/";

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
            content.SetValue(NavigationPropertiesConstants.NavigationNamePropName, "Notifications");
            content.SetValue(NavigationPropertiesConstants.IsHideFromLeftNavigationPropName, true);
            content.SetValue(NavigationPropertiesConstants.IsHideFromSubNavigationPropName, true);
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
            content.SetValue(NavigationPropertiesConstants.NavigationNamePropName, "Profile");
            content.SetValue(NavigationPropertiesConstants.IsHideFromLeftNavigationPropName, true);
            content.SetValue(NavigationPropertiesConstants.IsHideFromSubNavigationPropName, true);

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
            content.SetValue(NavigationPropertiesConstants.NavigationNamePropName, "Profile Edit");
            content.SetValue(NavigationPropertiesConstants.IsHideFromLeftNavigationPropName, true);
            content.SetValue(NavigationPropertiesConstants.IsHideFromSubNavigationPropName, true);

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
            content.SetValue(NavigationPropertiesConstants.NavigationNamePropName, "Search");
            content.SetValue(NavigationPropertiesConstants.IsHideFromLeftNavigationPropName, true);
            content.SetValue(NavigationPropertiesConstants.IsHideFromSubNavigationPropName, true);

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
            content.SetValue(NavigationPropertiesConstants.NavigationNamePropName, "Error");
            content.SetValue(NavigationPropertiesConstants.IsHideFromLeftNavigationPropName, true);
            content.SetValue(NavigationPropertiesConstants.IsHideFromSubNavigationPropName, true);

            SetGridValueAndSaveAndPublishContent(content, "errorPageGrid.json");
        }

        private void CreateNewsOverviewPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.HomePage));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.NewsOverviewPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("News", homePage.Id, DocumentTypeAliasConstants.NewsOverviewPage);

            SetGridValueAndSaveAndPublishContent(content, "newsOverviewPageGrid.json");
        }

        private void CreateNewsCreatePage()
        {
            var newsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.NewsOverviewPage));
            if (newsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.NewsCreatePage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Create", newsOverviewPage.Id, DocumentTypeAliasConstants.NewsCreatePage);

            SetGridValueAndSaveAndPublishContent(content, "newsCreatePageGrid.json");
        }

        private void CreateNewsEditPage()
        {
            var newsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.NewsOverviewPage));
            if (newsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.NewsEditPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Edit", newsOverviewPage.Id, DocumentTypeAliasConstants.NewsEditPage);

            SetGridValueAndSaveAndPublishContent(content, "newsEditPageGrid.json");
        }

        private void CreateNewsDetailsPage()
        {
            var newsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.NewsOverviewPage));
            if (newsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.NewsDetailsPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Details", newsOverviewPage.Id, DocumentTypeAliasConstants.NewsDetailsPage);

            SetGridValueAndSaveAndPublishContent(content, "newsDetailsPageGrid.json");
        }

        private void CreateBulletinsOverviewPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.HomePage));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.BulletinsOverviewPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Bulletins", homePage.Id, DocumentTypeAliasConstants.BulletinsOverviewPage);

            SetGridValueAndSaveAndPublishContent(content, "bulletinsOverviewPageGrid.json");
        }

        private void CreateBulletinsEditPage()
        {
            var eventsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.BulletinsOverviewPage));
            if (eventsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.BulletinsEditPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Edit", eventsOverviewPage.Id, DocumentTypeAliasConstants.BulletinsEditPage);

            SetGridValueAndSaveAndPublishContent(content, "bulletinsEditPageGrid.json");
        }

        private void CreateBulletinsDetailsPage()
        {
            var eventsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.BulletinsOverviewPage));
            if (eventsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.BulletinsDetailsPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Details", eventsOverviewPage.Id, DocumentTypeAliasConstants.BulletinsDetailsPage);
            SetGridValueAndSaveAndPublishContent(content, "bulletinsDetailsPageGrid.json");
        }

        private void CreateEventsOverviewPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.HomePage));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.EventsOverviewPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Events", homePage.Id, DocumentTypeAliasConstants.EventsOverviewPage);

            SetGridValueAndSaveAndPublishContent(content, "eventsOverviewPageGrid.json");
        }

        private void CreateEventsCreatePage()
        {
            var eventsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.EventsOverviewPage));
            if (eventsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.EventsCreatePage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Create", eventsOverviewPage.Id, DocumentTypeAliasConstants.EventsCreatePage);

            SetGridValueAndSaveAndPublishContent(content, "eventsCreatePageGrid.json");
        }

        private void CreateEventsEditPage()
        {
            var eventsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.EventsOverviewPage));
            if (eventsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.EventsEditPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Edit", eventsOverviewPage.Id, DocumentTypeAliasConstants.EventsEditPage);

            SetGridValueAndSaveAndPublishContent(content, "eventsEditPageGrid.json");
        }

        private void CreateEventsDetailsPage()
        {
            var eventsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.EventsOverviewPage));
            if (eventsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.EventsDetailsPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Details", eventsOverviewPage.Id, DocumentTypeAliasConstants.EventsDetailsPage);
            SetGridValueAndSaveAndPublishContent(content, "eventsDetailsPageGrid.json");
        }

        private void CreateDataFolder()
        {
            var dataFolder = _umbracoHelper.TypedContentAtRoot().FirstOrDefault(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.DataFolder));
            if (dataFolder != null)
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("Data folder", -1, DocumentTypeAliasConstants.DataFolder);

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateGlobalPanelFolder()
        {
            var dataFolder = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.DataFolder));
            if (dataFolder.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.GlobalPanelFolder)))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("Global Panel Folder", dataFolder.Id, DocumentTypeAliasConstants.GlobalPanelFolder);

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateSystemLinkFolder()
        {
            var dataFolder = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.DataFolder));
            if (dataFolder.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.SystemLinkFolder)))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("System Link Folder", dataFolder.Id, DocumentTypeAliasConstants.SystemLinkFolder);

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

            CoreInstallationStep.AddAllowedChildNode(DocumentTypeAliasConstants.MailTemplatesFolder, DocumentTypeAliasConstants.MailTemplate);
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
            if (dataFolder.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.MailTemplatesFolder)))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("Mail Templates Folder", dataFolder.Id, DocumentTypeAliasConstants.MailTemplatesFolder);

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateEventMailTemplate()
        {
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.DataFolder, DocumentTypeAliasConstants.MailTemplatesFolder));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.Event))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("Event", mailTemplatesFolder.Id, DocumentTypeAliasConstants.MailTemplate);
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.SubjectPropName, "Event");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.BodyPropName, "<p>Event</p><p>FullName: ##FullName##</p>");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.ExtraTokensPropName, "##FullName##");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName, NotificationTypeEnum.Event.ToString());

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
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.EventHided))
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
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName, NotificationTypeEnum.EventHided.ToString());

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
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.News))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("News", mailTemplatesFolder.Id, DocumentTypeAliasConstants.MailTemplate);
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.SubjectPropName, "News");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.BodyPropName, @"<p>News</p>
<p>FullName: ##FullName##</p>");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.ExtraTokensPropName, "##FullName##");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName, NotificationTypeEnum.News.ToString());

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