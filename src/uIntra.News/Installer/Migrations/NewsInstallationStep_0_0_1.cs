using uIntra.Core.Installer;
using uIntra.Core.Installer.Migrations;
using Umbraco.Core;

namespace uIntra.News.Installer.Migrations
{
    public class NewsInstallationStep: IIntranetInstallationStep
    {
        public string PackageName => "uIntra.News";
        public int Priority => 2;
        public string Version => InstallationVersionConstrants.Version_0_0_1;

        public void Execute()
        {
            CreateNewsOverviewPage();
            CreateNewsCreatePage();
            CreateNewsEditPage();
            CreateNewsDetailsPage();
        }

        private void CreateNewsOverviewPage()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var newsOverviewPage = contentService.GetContentType(NewsInstallationConstants.DocumentTypeAliases.NewsOverviewPage);
            if (newsOverviewPage != null) return;

            newsOverviewPage = CoreInstallationStep_0_0_1.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);
            //TODO: Move static methods to service

            newsOverviewPage.Name = NewsInstallationConstants.DocumentTypeNames.NewsOverviewPage;
            newsOverviewPage.Alias = NewsInstallationConstants.DocumentTypeAliases.NewsOverviewPage;
            newsOverviewPage.Icon = NewsInstallationConstants.DocumentTypeIcons.NewsOverviewPage;

            contentService.Save(newsOverviewPage);
            CoreInstallationStep_0_0_1.AddAllowedChildNode(CoreInstallationConstants.DocumentTypeAliases.HomePage, NewsInstallationConstants.DocumentTypeAliases.NewsOverviewPage);
        }

        private void CreateNewsCreatePage()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var newsCreatePage = contentService.GetContentType(NewsInstallationConstants.DocumentTypeAliases.NewsCreatePage);
            if (newsCreatePage != null) return;

            newsCreatePage = CoreInstallationStep_0_0_1.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);
            //TODO: Move static methods to service

            newsCreatePage.Name = NewsInstallationConstants.DocumentTypeNames.NewsCreatePage;
            newsCreatePage.Alias = NewsInstallationConstants.DocumentTypeAliases.NewsCreatePage;
            newsCreatePage.Icon = NewsInstallationConstants.DocumentTypeIcons.NewsCreatePage;

            contentService.Save(newsCreatePage);
            CoreInstallationStep_0_0_1.AddAllowedChildNode(NewsInstallationConstants.DocumentTypeAliases.NewsOverviewPage, NewsInstallationConstants.DocumentTypeAliases.NewsCreatePage);
        }
        private void CreateNewsEditPage()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var newsEditPage = contentService.GetContentType(NewsInstallationConstants.DocumentTypeAliases.NewsEditPage);
            if (newsEditPage != null) return;

            newsEditPage = CoreInstallationStep_0_0_1.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);
            //TODO: Move static methods to service

            newsEditPage.Name = NewsInstallationConstants.DocumentTypeNames.NewsEditPage;
            newsEditPage.Alias = NewsInstallationConstants.DocumentTypeAliases.NewsEditPage;
            newsEditPage.Icon = NewsInstallationConstants.DocumentTypeIcons.NewsEditPage;

            contentService.Save(newsEditPage);
            CoreInstallationStep_0_0_1.AddAllowedChildNode(NewsInstallationConstants.DocumentTypeAliases.NewsOverviewPage, NewsInstallationConstants.DocumentTypeAliases.NewsEditPage);
        }
        private void CreateNewsDetailsPage()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var newsDetailsPage = contentService.GetContentType(NewsInstallationConstants.DocumentTypeAliases.NewsDetailsPage);
            if (newsDetailsPage != null) return;

            newsDetailsPage = CoreInstallationStep_0_0_1.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);
            //TODO: Move static methods to service

            newsDetailsPage.Name = NewsInstallationConstants.DocumentTypeNames.NewsDetailsPage;
            newsDetailsPage.Alias = NewsInstallationConstants.DocumentTypeAliases.NewsDetailsPage;
            newsDetailsPage.Icon = NewsInstallationConstants.DocumentTypeIcons.NewsDetailsPage;

            contentService.Save(newsDetailsPage);
            CoreInstallationStep_0_0_1.AddAllowedChildNode(NewsInstallationConstants.DocumentTypeAliases.NewsOverviewPage, NewsInstallationConstants.DocumentTypeAliases.NewsDetailsPage);
        }

    }
}
