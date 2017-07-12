using uIntra.Core.Installer;
using Umbraco.Core;

namespace uIntra.Bulletins.Installer
{
    public class BulletinsInstallationStep: IIntranetInstallationStep
    {
        public string PackageName => "uIntra.Bulletins";
        public int Priority => 2;

        public void Execute()
        {
            CreateBulletinsOverviewPage();
            CreateBulletinsEditPage();
            CreateBulletinsDetailsPage();
        }

        private void CreateBulletinsOverviewPage()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var bulletinsOverviewPage = contentService.GetContentType(BulletinsInstallationConstants.DocumentTypeAliases.BulletinsOverviewPage);
            if (bulletinsOverviewPage != null) return;

            bulletinsOverviewPage = CoreInstallationStep.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);
            //TODO: Move static methods to service

            bulletinsOverviewPage.Name = BulletinsInstallationConstants.DocumentTypeNames.BulletinsOverviewPage;
            bulletinsOverviewPage.Alias = BulletinsInstallationConstants.DocumentTypeAliases.BulletinsOverviewPage;
            bulletinsOverviewPage.Icon = BulletinsInstallationConstants.DocumentTypeIcons.BulletinsOverviewPage;

            contentService.Save(bulletinsOverviewPage);
            CoreInstallationStep.AddAllowedChildNode(CoreInstallationConstants.DocumentTypeAliases.HomePage, BulletinsInstallationConstants.DocumentTypeAliases.BulletinsOverviewPage);
        }
        private void CreateBulletinsEditPage()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var bulletinsEditPage = contentService.GetContentType(BulletinsInstallationConstants.DocumentTypeAliases.BulletinsEditPage);
            if (bulletinsEditPage != null) return;

            bulletinsEditPage = CoreInstallationStep.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);
            //TODO: Move static methods to service

            bulletinsEditPage.Name = BulletinsInstallationConstants.DocumentTypeNames.BulletinsEditPage;
            bulletinsEditPage.Alias = BulletinsInstallationConstants.DocumentTypeAliases.BulletinsEditPage;
            bulletinsEditPage.Icon = BulletinsInstallationConstants.DocumentTypeIcons.BulletinsEditPage;

            contentService.Save(bulletinsEditPage);
            CoreInstallationStep.AddAllowedChildNode(BulletinsInstallationConstants.DocumentTypeAliases.BulletinsOverviewPage, BulletinsInstallationConstants.DocumentTypeAliases.BulletinsEditPage);
        }
        private void CreateBulletinsDetailsPage()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var bulletinsDetailsPage = contentService.GetContentType(BulletinsInstallationConstants.DocumentTypeAliases.BulletinsDetailsPage);
            if (bulletinsDetailsPage != null) return;

            bulletinsDetailsPage = CoreInstallationStep.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);
            //TODO: Move static methods to service

            bulletinsDetailsPage.Name = BulletinsInstallationConstants.DocumentTypeNames.BulletinsDetailsPage;
            bulletinsDetailsPage.Alias = BulletinsInstallationConstants.DocumentTypeAliases.BulletinsDetailsPage;
            bulletinsDetailsPage.Icon = BulletinsInstallationConstants.DocumentTypeIcons.BulletinsDetailsPage;

            contentService.Save(bulletinsDetailsPage);
            CoreInstallationStep.AddAllowedChildNode(BulletinsInstallationConstants.DocumentTypeAliases.BulletinsOverviewPage, BulletinsInstallationConstants.DocumentTypeAliases.BulletinsDetailsPage);
        }

    }
}
