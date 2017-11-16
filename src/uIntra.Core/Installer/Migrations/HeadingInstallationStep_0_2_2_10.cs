using Umbraco.Core;
using Umbraco.Core.Models;

namespace uIntra.Core.Installer.Migrations
{
    public class HeadingInstallationStep_0_2_2_10 : IIntranetInstallationStep
    {
        public string PackageName => "uIntra.Core";
        public int Priority => 0;
        public string Version => "0.2.2.10";

        public void Execute()
        {
            CreateHeading();
        }

        private void CreateHeading()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var heading = contentService.GetContentType(CoreInstallationConstants.DocumentTypeAliases.Heading);
            if (heading != null) return;

            var basePage = contentService.GetContentType(CoreInstallationConstants.DocumentTypeAliases.BasePage);

            heading = new ContentType(basePage.Id)
            {
                Name = CoreInstallationConstants.DocumentTypeNames.Heading,
                Alias = CoreInstallationConstants.DocumentTypeAliases.Heading,
                Icon = CoreInstallationConstants.DocumentTypeIcons.Heading
            };

            contentService.Save(heading);
            InstallationStepsHelper.AddAllowedChildNode(CoreInstallationConstants.DocumentTypeAliases.HomePage, CoreInstallationConstants.DocumentTypeAliases.Heading);
            InstallationStepsHelper.AddAllowedChildNode(CoreInstallationConstants.DocumentTypeAliases.Heading, CoreInstallationConstants.DocumentTypeAliases.ContentPage);
        }
    }
}
