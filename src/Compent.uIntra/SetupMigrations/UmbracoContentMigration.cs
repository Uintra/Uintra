using System;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using EmailWorker.Data.Services.Interfaces;
using uIntra.Core;
using uIntra.Core.Extentions;
using uIntra.Notification.Configuration;
using uIntra.Notification.Installer;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;
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

            //CreateMailTemplatesFolderDataType();
            //CreateMailWorkerDataTypes();
            //CreateMailTemplatesFolder();

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
            var homePage = _umbracoHelper.TypedContentAtRoot().FirstOrDefault(el => el.DocumentTypeAlias.Equals(HomePage.ModelTypeAlias));
            if (homePage != null)
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("Home Page", -1, HomePage.ModelTypeAlias);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.NavigationNamePropName, "Home Page");

            SetGridValueAndSaveAndPublishContent(content, "homePageGrid.json");
        }

        private void CreateNotificationPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(HomePage.ModelTypeAlias));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(NotificationPage.ModelTypeAlias)))
            {
                return;
            }

            var content = _contentService.CreateContent("Notifications", homePage.Id, NotificationPage.ModelTypeAlias);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.NavigationNamePropName, "Notifications");
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromLeftNavigationPropName, true);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromSubNavigationPropName, true);
            content.SetValue("itemCountForPopup", 5);

            SetGridValueAndSaveAndPublishContent(content, "notificationPageGrid.json");
        }

        private void CreateProfilePage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(HomePage.ModelTypeAlias));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(ProfilePage.ModelTypeAlias)))
            {
                return;
            }

            var content = _contentService.CreateContent("Profile", homePage.Id, ProfilePage.ModelTypeAlias);
            //content.SetValue(UmbracoContentMigrationConstants.Navigation.NavigationNamePropName, "Profile");
            //content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromLeftNavigationPropName, true);
            //content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromSubNavigationPropName, true);

            SetGridValueAndSaveAndPublishContent(content, "profilePageGrid.json");
        }

        private void CreateProfileEditPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(HomePage.ModelTypeAlias));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(ProfileEditPage.ModelTypeAlias)))
            {
                return;
            }

            var content = _contentService.CreateContent("Profile Edit Page", homePage.Id, ProfileEditPage.ModelTypeAlias);
            //content.SetValue(UmbracoContentMigrationConstants.Navigation.NavigationNamePropName, "Profile Edit");
            //content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromLeftNavigationPropName, true);
            //content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromSubNavigationPropName, true);

            SetGridValueAndSaveAndPublishContent(content, "profileEditPageGrid.json");
        }

        private void CreateSearchResultPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(HomePage.ModelTypeAlias));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(SearchResultPage.ModelTypeAlias)))
            {
                return;
            }

            var content = _contentService.CreateContent("Search Page", homePage.Id, SearchResultPage.ModelTypeAlias);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.NavigationNamePropName, "Search Page");
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromLeftNavigationPropName, true);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromSubNavigationPropName, true);

            SetGridValueAndSaveAndPublishContent(content, "searchResultPageGrid.json");
        }

        private void CreateErrorPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(HomePage.ModelTypeAlias));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(ErrorPage.ModelTypeAlias)))
            {
                return;
            }

            var content = _contentService.CreateContent("Error Page", homePage.Id, ErrorPage.ModelTypeAlias);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.NavigationNamePropName, "Error Page");
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromLeftNavigationPropName, true);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromSubNavigationPropName, true);

            SetGridValueAndSaveAndPublishContent(content, "errorPageGrid.json");
        }

        private void CreateNewsOverviewPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(HomePage.ModelTypeAlias));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(NewsOverviewPage.ModelTypeAlias)))
            {
                return;
            }

            var content = _contentService.CreateContent("News", homePage.Id, NewsOverviewPage.ModelTypeAlias);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.NavigationNamePropName, "News");
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromLeftNavigationPropName, false);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromSubNavigationPropName, false);

            SetGridValueAndSaveAndPublishContent(content, "newsOverviewPageGrid.json");
        }

        private void CreateNewsCreatePage()
        {
            var newsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(HomePage.ModelTypeAlias, NewsOverviewPage.ModelTypeAlias));
            if (newsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(NewsCreatePage.ModelTypeAlias)))
            {
                return;
            }

            var content = _contentService.CreateContent("Create", newsOverviewPage.Id, NewsCreatePage.ModelTypeAlias);

            SetGridValueAndSaveAndPublishContent(content, "newsCreatePageGrid.json");
        }

        private void CreateNewsEditPage()
        {
            var newsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(HomePage.ModelTypeAlias, NewsOverviewPage.ModelTypeAlias));
            if (newsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(NewsEditPage.ModelTypeAlias)))
            {
                return;
            }

            var content = _contentService.CreateContent("Edit", newsOverviewPage.Id, NewsEditPage.ModelTypeAlias);

            SetGridValueAndSaveAndPublishContent(content, "newsEditPageGrid.json");
        }

        private void CreateNewsDetailsPage()
        {
            var newsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(HomePage.ModelTypeAlias, NewsOverviewPage.ModelTypeAlias));
            if (newsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(NewsDetailsPage.ModelTypeAlias)))
            {
                return;
            }

            var content = _contentService.CreateContent("Details", newsOverviewPage.Id, NewsDetailsPage.ModelTypeAlias);

            SetGridValueAndSaveAndPublishContent(content, "newsDetailsPageGrid.json");
        }

        private void CreateBulletinsOverviewPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(HomePage.ModelTypeAlias));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(BulletinsOverviewPage.ModelTypeAlias)))
            {
                return;
            }

            var content = _contentService.CreateContent("Bulletins", homePage.Id, BulletinsOverviewPage.ModelTypeAlias);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.NavigationNamePropName, "Bulletins");
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromLeftNavigationPropName, false);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromSubNavigationPropName, false);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsShowInHomeNavigationPropName, true);

            SetGridValueAndSaveAndPublishContent(content, "bulletinsOverviewPageGrid.json");
        }

        private void CreateBulletinsEditPage()
        {
            var eventsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(HomePage.ModelTypeAlias, BulletinsOverviewPage.ModelTypeAlias));
            if (eventsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(BulletinsEditPage.ModelTypeAlias)))
            {
                return;
            }

            var content = _contentService.CreateContent("Edit", eventsOverviewPage.Id, BulletinsEditPage.ModelTypeAlias);

            SetGridValueAndSaveAndPublishContent(content, "bulletinsEditPageGrid.json");
        }

        private void CreateBulletinsDetailsPage()
        {
            var eventsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(HomePage.ModelTypeAlias, BulletinsOverviewPage.ModelTypeAlias));
            if (eventsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(BulletinsDetailsPage.ModelTypeAlias)))
            {
                return;
            }

            var content = _contentService.CreateContent("Details", eventsOverviewPage.Id, BulletinsDetailsPage.ModelTypeAlias);
            SetGridValueAndSaveAndPublishContent(content, "bulletinsDetailsPageGrid.json");
        }

        private void CreateEventsOverviewPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(HomePage.ModelTypeAlias));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(EventsOverviewPage.ModelTypeAlias)))
            {
                return;
            }

            var content = _contentService.CreateContent("Events", homePage.Id, EventsOverviewPage.ModelTypeAlias);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.NavigationNamePropName, "Events");
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromLeftNavigationPropName, false);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromSubNavigationPropName, false);

            SetGridValueAndSaveAndPublishContent(content, "eventsOverviewPageGrid.json");
        }

        private void CreateEventsCreatePage()
        {
            var eventsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(HomePage.ModelTypeAlias, EventsOverviewPage.ModelTypeAlias));
            if (eventsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(EventsCreatePage.ModelTypeAlias)))
            {
                return;
            }

            var content = _contentService.CreateContent("Create", eventsOverviewPage.Id, EventsCreatePage.ModelTypeAlias);

            SetGridValueAndSaveAndPublishContent(content, "eventsCreatePageGrid.json");
        }

        private void CreateEventsEditPage()
        {
            var eventsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(HomePage.ModelTypeAlias, EventsOverviewPage.ModelTypeAlias));
            if (eventsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(EventsEditPage.ModelTypeAlias)))
            {
                return;
            }

            var content = _contentService.CreateContent("Edit", eventsOverviewPage.Id, EventsEditPage.ModelTypeAlias);

            SetGridValueAndSaveAndPublishContent(content, "eventsEditPageGrid.json");
        }

        private void CreateEventsDetailsPage()
        {
            var eventsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(HomePage.ModelTypeAlias, EventsOverviewPage.ModelTypeAlias));
            if (eventsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(EventsDetailsPage.ModelTypeAlias)))
            {
                return;
            }

            var content = _contentService.CreateContent("Details", eventsOverviewPage.Id, EventsDetailsPage.ModelTypeAlias);
            SetGridValueAndSaveAndPublishContent(content, "eventsDetailsPageGrid.json");
        }

        private void CreateDataFolder()
        {
            var dataFolder = _umbracoHelper.TypedContentAtRoot().FirstOrDefault(el => el.DocumentTypeAlias.Equals(DataFolder.ModelTypeAlias));
            if (dataFolder != null)
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("Data folder", -1, DataFolder.ModelTypeAlias);

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateGlobalPanelFolder()
        {
            var dataFolder = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(DataFolder.ModelTypeAlias));
            if (dataFolder.Children.Any(el => el.DocumentTypeAlias.Equals(GlobalPanelFolder.ModelTypeAlias)))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("Global Panel Folder", dataFolder.Id, GlobalPanelFolder.ModelTypeAlias);

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateSystemLinkFolder()
        {
            var dataFolder = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(DataFolder.ModelTypeAlias));
            if (dataFolder.Children.Any(el => el.DocumentTypeAlias.Equals(SystemLinkFolder.ModelTypeAlias)))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("System Link Folder", dataFolder.Id, SystemLinkFolder.ModelTypeAlias);

            _contentService.SaveAndPublishWithStatus(content);
        }

        //private void CreateMailWorkerDataTypes()
        //{
        //    var contentService = ApplicationContext.Current.Services.ContentTypeService;
        //    var dataTypeService = ApplicationContext.Current.Services.DataTypeService;

        //    var mailTemplateDocType = contentService.GetContentType(MailTemplate.ModelTypeAlias);
        //    if (mailTemplateDocType != null) return;
           
        //    var dataContentFolder = contentService.GetContentTypeContainers(CoreInstallationConstants.DocumentTypesContainerNames.DataContent, 1).First();
        //    sentMailsDocumentTypeService.CreateMailTemplateDocTypes(dataContentFolder.Id.ToString());

        //    mailTemplateDocType = contentService.GetContentType(MailTemplate.ModelTypeAlias);

        //    mailTemplateDocType.RemovePropertyType(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName);
        //    var notificationTypeEnumDropdown = dataTypeService.GetDataTypeDefinitionByName(NotificationInstallationConstants.DataTypeNames.NotificationTypeEnum);
        //    var newEmailTypeProperty = new PropertyType(notificationTypeEnumDropdown)
        //    {
        //        Name = "Email type",
        //        Alias= "emailType"
        //    };

        //    mailTemplateDocType.AddPropertyType(newEmailTypeProperty, "Content");
        //    contentService.Save(mailTemplateDocType);

        //    CoreInstallationStep.AddAllowedChildNode(MailTemplatesFolder.ModelTypeAlias, MailTemplate.ModelTypeAlias);
        //}
        //private void CreateMailTemplatesFolderDataType()
        //{
        //    var contentService = ApplicationContext.Current.Services.ContentTypeService;
        //    var mailTemplateFolderDataType = contentService.GetContentType(MailTemplatesFolder.ModelTypeAlias);
        //    if (mailTemplateFolderDataType != null) return;

        //    var dataContentFolder = contentService.GetContentTypeContainers(CoreInstallationConstants.DocumentTypesContainerNames.Folders, 1).First();

        //    mailTemplateFolderDataType = new ContentType(dataContentFolder.Id)
        //    {
        //        Name = UmbracoContentMigrationConstants.MailTemplate.MailTemplatesFolderName,
        //        Alias = MailTemplatesFolder.ModelTypeAlias
        //    };

        //    contentService.Save(mailTemplateFolderDataType);

        //}
        //private void CreateMailTemplatesFolder()
        //{
        //    var dataFolder = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(DataFolder.ModelTypeAlias));
        //    if (dataFolder.Children.Any(el => el.DocumentTypeAlias.Equals(MailTemplatesFolder.ModelTypeAlias)))
        //    {
        //        return;
        //    }

        //    var content = _contentService.CreateContentWithIdentity("Mail Templates Folder", dataFolder.Id, MailTemplatesFolder.ModelTypeAlias);

        //    _contentService.SaveAndPublishWithStatus(content);
        //}

        private void CreateEventMailTemplate()
        {
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DataFolder.ModelTypeAlias, MailTemplatesFolder.ModelTypeAlias));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.Event))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("Event", mailTemplatesFolder.Id, MailTemplate.ModelTypeAlias);
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.SubjectPropName, "Event");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.BodyPropName, "<p>Event</p><p>FullName: ##FullName##</p>");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.ExtraTokensPropName, "##FullName##");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName, NotificationTypeEnum.Event.ToString());

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateEventUpdatedMailTemplate()
        {
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DataFolder.ModelTypeAlias, MailTemplatesFolder.ModelTypeAlias));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.EventUpdated))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("EventUpdated", mailTemplatesFolder.Id, MailTemplate.ModelTypeAlias);
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
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DataFolder.ModelTypeAlias, MailTemplatesFolder.ModelTypeAlias));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.EventHided))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("EventHided", mailTemplatesFolder.Id, MailTemplate.ModelTypeAlias);
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
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DataFolder.ModelTypeAlias, MailTemplatesFolder.ModelTypeAlias));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.BeforeStart))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("BeforeStart", mailTemplatesFolder.Id, MailTemplate.ModelTypeAlias);
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
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DataFolder.ModelTypeAlias, MailTemplatesFolder.ModelTypeAlias));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.News))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("News", mailTemplatesFolder.Id, MailTemplate.ModelTypeAlias);
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.SubjectPropName, "News");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.BodyPropName, @"<p>News</p>
<p>FullName: ##FullName##</p>");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.ExtraTokensPropName, "##FullName##");
            content.SetValue(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName, NotificationTypeEnum.News.ToString());

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateActivityLikeAddedMailTemplate()
        {
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DataFolder.ModelTypeAlias, MailTemplatesFolder.ModelTypeAlias));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.ActivityLikeAdded))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("ActivityLikeAdded", mailTemplatesFolder.Id, MailTemplate.ModelTypeAlias);
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
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DataFolder.ModelTypeAlias, MailTemplatesFolder.ModelTypeAlias));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.CommentAdded))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("CommentAdded", mailTemplatesFolder.Id, MailTemplate.ModelTypeAlias);
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
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DataFolder.ModelTypeAlias, MailTemplatesFolder.ModelTypeAlias));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.CommentEdited))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("CommentEdited", mailTemplatesFolder.Id, MailTemplate.ModelTypeAlias);
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
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DataFolder.ModelTypeAlias, MailTemplatesFolder.ModelTypeAlias));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.CommentReplied))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("CommentReplied", mailTemplatesFolder.Id, MailTemplate.ModelTypeAlias);
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
            var mailTemplatesFolder = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DataFolder.ModelTypeAlias, MailTemplatesFolder.ModelTypeAlias));
            if (mailTemplatesFolder.Children.Any(el => el.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName) == NotificationTypeEnum.CommentLikeAdded))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("CommentLikeAdded", mailTemplatesFolder.Id, MailTemplate.ModelTypeAlias);
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