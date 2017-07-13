using uIntra.Core.Installer;
using Umbraco.Core;

namespace uIntra.News.Installer
{
    public class NewsInstallationStep: IIntranetInstallationStep
    {
        public string PackageName => "uIntra.News";
        public int Priority => 2;

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

            newsOverviewPage = CoreInstallationStep.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);
            //TODO: Move static methods to service

            newsOverviewPage.Name = NewsInstallationConstants.DocumentTypeNames.NewsOverviewPage;
            newsOverviewPage.Alias = NewsInstallationConstants.DocumentTypeAliases.NewsOverviewPage;
            newsOverviewPage.Icon = NewsInstallationConstants.DocumentTypeIcons.NewsOverviewPage;

            contentService.Save(newsOverviewPage);
            CoreInstallationStep.AddAllowedChildNode(CoreInstallationConstants.DocumentTypeAliases.HomePage, NewsInstallationConstants.DocumentTypeAliases.NewsOverviewPage);
        }

        private void CreateNewsCreatePage()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var newsCreatePage = contentService.GetContentType(NewsInstallationConstants.DocumentTypeAliases.NewsCreatePage);
            if (newsCreatePage != null) return;

            newsCreatePage = CoreInstallationStep.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);
            //TODO: Move static methods to service

            newsCreatePage.Name = NewsInstallationConstants.DocumentTypeNames.NewsCreatePage;
            newsCreatePage.Alias = NewsInstallationConstants.DocumentTypeAliases.NewsCreatePage;
            newsCreatePage.Icon = NewsInstallationConstants.DocumentTypeIcons.NewsCreatePage;

            contentService.Save(newsCreatePage);
            CoreInstallationStep.AddAllowedChildNode(NewsInstallationConstants.DocumentTypeAliases.NewsOverviewPage, NewsInstallationConstants.DocumentTypeAliases.NewsCreatePage);
        }
        private void CreateNewsEditPage()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var newsEditPage = contentService.GetContentType(NewsInstallationConstants.DocumentTypeAliases.NewsEditPage);
            if (newsEditPage != null) return;

            newsEditPage = CoreInstallationStep.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);
            //TODO: Move static methods to service

            newsEditPage.Name = NewsInstallationConstants.DocumentTypeNames.NewsEditPage;
            newsEditPage.Alias = NewsInstallationConstants.DocumentTypeAliases.NewsEditPage;
            newsEditPage.Icon = NewsInstallationConstants.DocumentTypeIcons.NewsEditPage;

            contentService.Save(newsEditPage);
            CoreInstallationStep.AddAllowedChildNode(NewsInstallationConstants.DocumentTypeAliases.NewsOverviewPage, NewsInstallationConstants.DocumentTypeAliases.NewsEditPage);
        }
        private void CreateNewsDetailsPage()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var newsDetailsPage = contentService.GetContentType(NewsInstallationConstants.DocumentTypeAliases.NewsDetailsPage);
            if (newsDetailsPage != null) return;

            newsDetailsPage = CoreInstallationStep.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);
            //TODO: Move static methods to service

            newsDetailsPage.Name = NewsInstallationConstants.DocumentTypeNames.NewsDetailsPage;
            newsDetailsPage.Alias = NewsInstallationConstants.DocumentTypeAliases.NewsDetailsPage;
            newsDetailsPage.Icon = NewsInstallationConstants.DocumentTypeIcons.NewsDetailsPage;

            contentService.Save(newsDetailsPage);
            CoreInstallationStep.AddAllowedChildNode(NewsInstallationConstants.DocumentTypeAliases.NewsOverviewPage, NewsInstallationConstants.DocumentTypeAliases.NewsDetailsPage);
        }

    }
}
