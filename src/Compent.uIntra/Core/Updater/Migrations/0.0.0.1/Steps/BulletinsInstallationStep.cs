using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Steps.AggregateSubsteps;
using static Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants.BulletinsInstallationConstants;
using static Compent.Uintra.Core.Updater.ExecutionResult;

namespace Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Steps
{
    public class BulletinsInstallationStep : IMigrationStep
    {
        public ExecutionResult Execute()
        {
            CreateBulletinsOverviewPage();
            CreateBulletinsEditPage();
            CreateBulletinsDetailsPage();
            return Success;
        }

        public void Undo()
        {
            throw new System.NotImplementedException();
        }

        private void CreateBulletinsOverviewPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = DocumentTypeNames.BulletinsOverviewPage,
                Alias = DocumentTypeAliases.BulletinsOverviewPage,
                Icon = DocumentTypeIcons.BulletinsOverviewPage,
                ParentAlias = CoreInstallationConstants.DocumentTypeAliases.HomePage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateBulletinsEditPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = DocumentTypeNames.BulletinsEditPage,
                Alias = DocumentTypeAliases.BulletinsEditPage,
                Icon = DocumentTypeIcons.BulletinsEditPage,
                ParentAlias = DocumentTypeAliases.BulletinsOverviewPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);

        }

        private void CreateBulletinsDetailsPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = DocumentTypeNames.BulletinsDetailsPage,
                Alias = DocumentTypeAliases.BulletinsDetailsPage,
                Icon = DocumentTypeIcons.BulletinsDetailsPage,
                ParentAlias = DocumentTypeAliases.BulletinsOverviewPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);

        }
    }
}