using uIntra.Core.Installer;

namespace uIntra.Bulletins.Installer.Migrations
{
    public class BulletinsInstallationStep_0_0_1 : IIntranetInstallationStep
    {
        public string PackageName => "uIntra.Bulletins";
        public int Priority => 2;
        public string Version => InstallationVersionConstrants.Version_0_0_1;

        public void Execute()
        {
            CreateBulletinsOverviewPage();
            CreateBulletinsEditPage();
            CreateBulletinsDetailsPage();
        }

        private void CreateBulletinsOverviewPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = BulletinsInstallationConstants.DocumentTypeNames.BulletinsOverviewPage,
                Alias = BulletinsInstallationConstants.DocumentTypeAliases.BulletinsOverviewPage,
                Icon = BulletinsInstallationConstants.DocumentTypeIcons.BulletinsOverviewPage,
                ParentAlias = CoreInstallationConstants.DocumentTypeAliases.HomePage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }
        private void CreateBulletinsEditPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = BulletinsInstallationConstants.DocumentTypeNames.BulletinsEditPage,
                Alias = BulletinsInstallationConstants.DocumentTypeAliases.BulletinsEditPage,
                Icon = BulletinsInstallationConstants.DocumentTypeIcons.BulletinsEditPage,
                ParentAlias = BulletinsInstallationConstants.DocumentTypeAliases.BulletinsOverviewPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);

        }
        private void CreateBulletinsDetailsPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = BulletinsInstallationConstants.DocumentTypeNames.BulletinsDetailsPage,
                Alias = BulletinsInstallationConstants.DocumentTypeAliases.BulletinsDetailsPage,
                Icon = BulletinsInstallationConstants.DocumentTypeIcons.BulletinsDetailsPage,
                ParentAlias = BulletinsInstallationConstants.DocumentTypeAliases.BulletinsOverviewPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);

        }

    }
}
