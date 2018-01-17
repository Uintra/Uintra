using uIntra.Core.Installer;

namespace uIntra.Events.Installer.Migrations
{
    public class EventsInstallationStep_0_0_1 : IIntranetInstallationStep
    {
        public string PackageName => "uIntra.Events";
        public int Priority => 2;
        public string Version => InstallationVersionConstrants.Version_0_0_1;

        public void Execute()
        {
            CreateEventsOverviewPage();
            CreateEventsCreatePage();
            CreateEventsEditPage();
            CreateEventsDetailsPage();
        }

        private void CreateEventsOverviewPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = EventsInstallationConstants.DocumentTypeNames.EventsOverviewPage,
                Alias = EventsInstallationConstants.DocumentTypeAliases.EventsOverviewPage,
                Icon = EventsInstallationConstants.DocumentTypeIcons.EventsOverviewPage,
                ParentAlias = CoreInstallationConstants.DocumentTypeAliases.HomePage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateEventsCreatePage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = EventsInstallationConstants.DocumentTypeNames.EventsCreatePage,
                Alias = EventsInstallationConstants.DocumentTypeAliases.EventsCreatePage,
                Icon = EventsInstallationConstants.DocumentTypeIcons.EventsCreatePage,
                ParentAlias = EventsInstallationConstants.DocumentTypeAliases.EventsOverviewPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateEventsEditPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = EventsInstallationConstants.DocumentTypeNames.EventsEditPage,
                Alias = EventsInstallationConstants.DocumentTypeAliases.EventsEditPage,
                Icon = EventsInstallationConstants.DocumentTypeIcons.EventsEditPage,
                ParentAlias = EventsInstallationConstants.DocumentTypeAliases.EventsOverviewPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateEventsDetailsPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = EventsInstallationConstants.DocumentTypeNames.EventsDetailsPage,
                Alias = EventsInstallationConstants.DocumentTypeAliases.EventsDetailsPage,
                Icon = EventsInstallationConstants.DocumentTypeIcons.EventsDetailsPage,
                ParentAlias = EventsInstallationConstants.DocumentTypeAliases.EventsOverviewPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

    }
}
