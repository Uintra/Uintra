using uIntra.Core.Installer;
using Compent.uIntra.Core.Updater.Migrations._0._0._0._1.Constants;
using Compent.uIntra.Core.Updater.Migrations._0._0._0._1.Steps.AggregateSubsteps;
using static Compent.uIntra.Core.Updater.Migrations._0._0._0._1.Constants.EventsInstallationConstants;
using static Compent.uIntra.Core.Updater.ExecutionResult;

namespace Compent.uIntra.Core.Updater.Migrations._0._0._0._1.Steps
{
    public class EventsInstallationStep : IMigrationStep
    {

        public ExecutionResult Execute()
        {

            CreateEventsOverviewPage();
            CreateEventsCreatePage();
            CreateEventsEditPage();
            CreateEventsDetailsPage();
            return Success;
        }

        public void Undo()
        {
            throw new System.NotImplementedException();
        }

        private void CreateEventsOverviewPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = DocumentTypeNames.EventsOverviewPage,
                Alias = DocumentTypeAliases.EventsOverviewPage,
                Icon = DocumentTypeIcons.EventsOverviewPage,
                ParentAlias = CoreInstallationConstants.DocumentTypeAliases.HomePage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateEventsCreatePage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = DocumentTypeNames.EventsCreatePage,
                Alias = DocumentTypeAliases.EventsCreatePage,
                Icon = DocumentTypeIcons.EventsCreatePage,
                ParentAlias = DocumentTypeAliases.EventsOverviewPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateEventsEditPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = DocumentTypeNames.EventsEditPage,
                Alias = DocumentTypeAliases.EventsEditPage,
                Icon = DocumentTypeIcons.EventsEditPage,
                ParentAlias = DocumentTypeAliases.EventsOverviewPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateEventsDetailsPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = DocumentTypeNames.EventsDetailsPage,
                Alias = DocumentTypeAliases.EventsDetailsPage,
                Icon = DocumentTypeIcons.EventsDetailsPage,
                ParentAlias = DocumentTypeAliases.EventsOverviewPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }
    }
}