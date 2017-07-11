using System;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using uIntra.Core.Extentions;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Compent.uIntra.SetupMigrations
{
    public class UmbracoContentMigration
    {
        private const string JsonFilesFolder = "~/SetupMigrations/json/";

        private readonly UmbracoHelper _umbracoHelper;
        private readonly IContentService _contentService;

        public UmbracoContentMigration(UmbracoHelper umbracoHelper, IContentService contentService)
        {
            _umbracoHelper = umbracoHelper;
            _contentService = contentService;
        }

        public void Init()
        {
            CreateHomePage();
            CreateNotificationPage();
            CreateProfilePage();
            CreateProfileEditPage();
            CreateSearchResultPage();

            CreateNewsOverviewPage();
            CreateNewsCreatePage();
            CreateNewsEditPage();
            CreateNewsDetailsPage();

            CreateEventsOverviewPage();
            CreateEventsCreatePage();
            CreateEventsEditPage();
            CreateEventsDetailsPage();
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

            var gridContent = GetGridContent("homePageGrid.json");
            content.SetValue(UmbracoContentMigrationConstants.Grid.GridPropName, gridContent);

            _contentService.SaveAndPublishWithStatus(content);
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

            var gridContent = GetGridContent("notificationPageGrid.json");
            content.SetValue(UmbracoContentMigrationConstants.Grid.GridPropName, gridContent);

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateProfilePage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.ProfilePageDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContent("Profile", homePage.Id, UmbracoContentMigrationConstants.DocType.ProfilePageDocTypeName);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.NavigationNamePropName, "Profile");
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromLeftNavigationPropName, true);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromSubNavigationPropName, true);

            var gridContent = GetGridContent("profilePageGrid.json");
            content.SetValue(UmbracoContentMigrationConstants.Grid.GridPropName, gridContent);

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateProfileEditPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.ProfileEditPageDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContent("Profile Edit Page", homePage.Id, UmbracoContentMigrationConstants.DocType.ProfileEditPageDocTypeName);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.NavigationNamePropName, "Profile Edit");
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromLeftNavigationPropName, true);
            content.SetValue(UmbracoContentMigrationConstants.Navigation.IsHideFromSubNavigationPropName, true);

            var gridContent = GetGridContent("profileEditPageGrid.json");
            content.SetValue(UmbracoContentMigrationConstants.Grid.GridPropName, gridContent);

            _contentService.SaveAndPublishWithStatus(content);
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

            var gridContent = GetGridContent("searchResultPageGrid.json");
            content.SetValue(UmbracoContentMigrationConstants.Grid.GridPropName, gridContent);

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateNewsOverviewPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.NewsOverviewPageDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContent("News", homePage.Id, UmbracoContentMigrationConstants.DocType.NewsOverviewPageDocTypeName);

            var gridContent = GetGridContent("newsOverviewPageGrid.json");
            content.SetValue(UmbracoContentMigrationConstants.Grid.GridPropName, gridContent);

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateNewsCreatePage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName));
            var newsOverviewPage = homePage.Children.Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.NewsOverviewPageDocTypeName));
            if (newsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.NewsCreatePageDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContent("Create", newsOverviewPage.Id, UmbracoContentMigrationConstants.DocType.NewsCreatePageDocTypeName);

            var gridContent = GetGridContent("newsCreatePageGrid.json");
            content.SetValue(UmbracoContentMigrationConstants.Grid.GridPropName, gridContent);

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateNewsEditPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName));
            var newsOverviewPage = homePage.Children.Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.NewsOverviewPageDocTypeName));
            if (newsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.NewsEditPageDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContent("Edit", newsOverviewPage.Id, UmbracoContentMigrationConstants.DocType.NewsEditPageDocTypeName);

            var gridContent = GetGridContent("newsEditPageGrid.json");
            content.SetValue(UmbracoContentMigrationConstants.Grid.GridPropName, gridContent);

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateNewsDetailsPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName));
            var newsOverviewPage = homePage.Children.Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.NewsOverviewPageDocTypeName));
            if (newsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.NewsDetailsPageDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContent("Details", newsOverviewPage.Id, UmbracoContentMigrationConstants.DocType.NewsDetailsPageDocTypeName);

            var gridContent = GetGridContent("newsDetailsPageGrid.json");
            content.SetValue(UmbracoContentMigrationConstants.Grid.GridPropName, gridContent);

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateEventsOverviewPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName));
            if (homePage.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.EventsOverviewPageDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContent("Events", homePage.Id, UmbracoContentMigrationConstants.DocType.EventsOverviewPageDocTypeName);

            var gridContent = GetGridContent("eventsOverviewPageGrid.json");
            content.SetValue(UmbracoContentMigrationConstants.Grid.GridPropName, gridContent);

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateEventsCreatePage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName));
            var eventsOverviewPage = homePage.Children.Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.EventsOverviewPageDocTypeName));
            if (eventsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.EventsCreatePageDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContent("Create", eventsOverviewPage.Id, UmbracoContentMigrationConstants.DocType.EventsCreatePageDocTypeName);

            var gridContent = GetGridContent("eventsCreatePageGrid.json");
            content.SetValue(UmbracoContentMigrationConstants.Grid.GridPropName, gridContent);

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateEventsEditPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName));
            var eventsOverviewPage = homePage.Children.Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.EventsOverviewPageDocTypeName));
            if (eventsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.EventsEditPageDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContent("Edit", eventsOverviewPage.Id, UmbracoContentMigrationConstants.DocType.EventsEditPageDocTypeName);

            var gridContent = GetGridContent("eventsEditPageGrid.json");
            content.SetValue(UmbracoContentMigrationConstants.Grid.GridPropName, gridContent);

            _contentService.SaveAndPublishWithStatus(content);
        }

        private void CreateEventsDetailsPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.HomePageDocTypeName));
            var eventsOverviewPage = homePage.Children.Single(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.EventsOverviewPageDocTypeName));
            if (eventsOverviewPage.Children.Any(el => el.DocumentTypeAlias.Equals(UmbracoContentMigrationConstants.DocType.EventsDetailsPageDocTypeName)))
            {
                return;
            }

            var content = _contentService.CreateContent("Details", eventsOverviewPage.Id, UmbracoContentMigrationConstants.DocType.EventsDetailsPageDocTypeName);

            var gridContent = GetGridContent("eventsDetailsPageGrid.json");
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
        }

        public static class Navigation
        {
            public const string NavigationNamePropName = "navigationName";
            public const string IsHideFromLeftNavigationPropName = "isHideFromLeftNavigation";
            public const string IsHideFromSubNavigationPropName = "isHideFromSubNavigation";
        }

        public static class Grid
        {
            public const string GridPropName = "grid";
        }
    }
}