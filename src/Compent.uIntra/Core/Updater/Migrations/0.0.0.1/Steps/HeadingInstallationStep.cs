using Umbraco.Core;
using Umbraco.Core.Models;
using static Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants.CoreInstallationConstants;
using static Compent.Uintra.Core.Updater.ExecutionResult;

namespace Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Steps
{
    public class HeadingInstallationStep : IMigrationStep
    {
        ExecutionResult IMigrationStep.Execute()
        {
            CreateHeading();
            return Success;
        }

        public void Undo()
        {
            throw new System.NotImplementedException();
        }

        private void CreateHeading()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var heading = contentService.GetContentType(DocumentTypeAliases.Heading);
            if (heading != null) return;

            var basePage = contentService.GetContentType(DocumentTypeAliases.BasePage);

            heading = new ContentType(basePage.Id)
            {
                Name = DocumentTypeNames.Heading,
                Alias = DocumentTypeAliases.Heading,
                Icon = DocumentTypeIcons.Heading
            };

            contentService.Save(heading);
            InstallationStepsHelper.AddAllowedChildNode(DocumentTypeAliases.HomePage, DocumentTypeAliases.Heading);
            InstallationStepsHelper.AddAllowedChildNode(DocumentTypeAliases.Heading, DocumentTypeAliases.ContentPage);
        }
    }
}
