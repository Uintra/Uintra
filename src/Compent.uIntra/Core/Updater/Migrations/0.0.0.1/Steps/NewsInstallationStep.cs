using System.Linq;
using Uintra.Core.Installer;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Steps.AggregateSubsteps;
using static Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants.NewsInstallationConstants;
using static Compent.Uintra.Core.Updater.ExecutionResult;

namespace Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Steps
{
    public class NewsInstallationStep : IMigrationStep
    {
        public ExecutionResult Execute()
        {
            CreateNewsOverviewPage();
            CreateNewsCreatePage();
            CreateNewsEditPage();
            CreateNewsDetailsPage();

            return Success;
        }

        public void Undo()
        {
            throw new System.NotImplementedException();
        }

        private void CreateNewsOverviewPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = DocumentTypeNames.NewsOverviewPage,
                Alias = DocumentTypeAliases.NewsOverviewPage,
                Icon = DocumentTypeIcons.NewsOverviewPage,
                ParentAlias = CoreInstallationConstants.DocumentTypeAliases.HomePage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateNewsCreatePage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = DocumentTypeNames.NewsCreatePage,
                Alias = DocumentTypeAliases.NewsCreatePage,
                Icon = DocumentTypeIcons.NewsCreatePage,
                ParentAlias = DocumentTypeAliases.NewsOverviewPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateNewsEditPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = DocumentTypeNames.NewsEditPage,
                Alias = DocumentTypeAliases.NewsEditPage,
                Icon = DocumentTypeIcons.NewsEditPage,
                ParentAlias = DocumentTypeAliases.NewsOverviewPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateNewsDetailsPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = DocumentTypeNames.NewsDetailsPage,
                Alias = DocumentTypeAliases.NewsDetailsPage,
                Icon = DocumentTypeIcons.NewsDetailsPage,
                ParentAlias = DocumentTypeAliases.NewsOverviewPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }
    }
}