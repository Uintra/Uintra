using uIntra.Core.Installer;

namespace uIntra.News.Installer.Migrations
{
    public class NewsInstallationStep : IIntranetInstallationStep
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
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = NewsInstallationConstants.DocumentTypeNames.NewsOverviewPage,
                Alias = NewsInstallationConstants.DocumentTypeAliases.NewsOverviewPage,
                Icon = NewsInstallationConstants.DocumentTypeIcons.NewsOverviewPage,
                ParentAlias = CoreInstallationConstants.DocumentTypeAliases.HomePage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateNewsCreatePage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = NewsInstallationConstants.DocumentTypeNames.NewsCreatePage,
                Alias = NewsInstallationConstants.DocumentTypeAliases.NewsCreatePage,
                Icon = NewsInstallationConstants.DocumentTypeIcons.NewsCreatePage,
                ParentAlias = NewsInstallationConstants.DocumentTypeAliases.NewsOverviewPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }
        private void CreateNewsEditPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = NewsInstallationConstants.DocumentTypeNames.NewsEditPage,
                Alias = NewsInstallationConstants.DocumentTypeAliases.NewsEditPage,
                Icon = NewsInstallationConstants.DocumentTypeIcons.NewsEditPage,
                ParentAlias = NewsInstallationConstants.DocumentTypeAliases.NewsOverviewPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }
        private void CreateNewsDetailsPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = NewsInstallationConstants.DocumentTypeNames.NewsDetailsPage,
                Alias = NewsInstallationConstants.DocumentTypeAliases.NewsDetailsPage,
                Icon = NewsInstallationConstants.DocumentTypeIcons.NewsDetailsPage,
                ParentAlias = NewsInstallationConstants.DocumentTypeAliases.NewsOverviewPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

    }
}
