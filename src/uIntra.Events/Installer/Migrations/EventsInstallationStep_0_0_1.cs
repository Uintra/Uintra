using uIntra.Core.Installer;
using uIntra.Core.Installer.Migrations;
using Umbraco.Core;

namespace uIntra.Events.Installer.Migrations
{
    public class EventsInstallationStep_0_0_1: IIntranetInstallationStep
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
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var eventsOverviewPage = contentService.GetContentType(EventsInstallationConstants.DocumentTypeAliases.EventsOverviewPage);
            if (eventsOverviewPage != null) return;

            eventsOverviewPage = CoreInstallationStep_0_0_1.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);
            //TODO: Move static methods to service

            eventsOverviewPage.Name = EventsInstallationConstants.DocumentTypeNames.EventsOverviewPage;
            eventsOverviewPage.Alias = EventsInstallationConstants.DocumentTypeAliases.EventsOverviewPage;
            eventsOverviewPage.Icon = EventsInstallationConstants.DocumentTypeIcons.EventsOverviewPage;

            contentService.Save(eventsOverviewPage);
            CoreInstallationStep_0_0_1.AddAllowedChildNode(CoreInstallationConstants.DocumentTypeAliases.HomePage, EventsInstallationConstants.DocumentTypeAliases.EventsOverviewPage);
        }

        private void CreateEventsCreatePage()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var eventsCreatePage = contentService.GetContentType(EventsInstallationConstants.DocumentTypeAliases.EventsCreatePage);
            if (eventsCreatePage != null) return;

            eventsCreatePage = CoreInstallationStep_0_0_1.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);
            //TODO: Move static methods to service

            eventsCreatePage.Name = EventsInstallationConstants.DocumentTypeNames.EventsCreatePage;
            eventsCreatePage.Alias = EventsInstallationConstants.DocumentTypeAliases.EventsCreatePage;
            eventsCreatePage.Icon = EventsInstallationConstants.DocumentTypeIcons.EventsCreatePage;

            contentService.Save(eventsCreatePage);
            CoreInstallationStep_0_0_1.AddAllowedChildNode(EventsInstallationConstants.DocumentTypeAliases.EventsOverviewPage, EventsInstallationConstants.DocumentTypeAliases.EventsCreatePage);
        }
        private void CreateEventsEditPage()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var eventsEditPage = contentService.GetContentType(EventsInstallationConstants.DocumentTypeAliases.EventsEditPage);
            if (eventsEditPage != null) return;

            eventsEditPage = CoreInstallationStep_0_0_1.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);
            //TODO: Move static methods to service

            eventsEditPage.Name = EventsInstallationConstants.DocumentTypeNames.EventsEditPage;
            eventsEditPage.Alias = EventsInstallationConstants.DocumentTypeAliases.EventsEditPage;
            eventsEditPage.Icon = EventsInstallationConstants.DocumentTypeIcons.EventsEditPage;

            contentService.Save(eventsEditPage);
            CoreInstallationStep_0_0_1.AddAllowedChildNode(EventsInstallationConstants.DocumentTypeAliases.EventsOverviewPage, EventsInstallationConstants.DocumentTypeAliases.EventsEditPage);
        }
        private void CreateEventsDetailsPage()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var eventsDetailsPage = contentService.GetContentType(EventsInstallationConstants.DocumentTypeAliases.EventsDetailsPage);
            if (eventsDetailsPage != null) return;

            eventsDetailsPage = CoreInstallationStep_0_0_1.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);
            //TODO: Move static methods to service

            eventsDetailsPage.Name = EventsInstallationConstants.DocumentTypeNames.EventsDetailsPage;
            eventsDetailsPage.Alias = EventsInstallationConstants.DocumentTypeAliases.EventsDetailsPage;
            eventsDetailsPage.Icon = EventsInstallationConstants.DocumentTypeIcons.EventsDetailsPage;

            contentService.Save(eventsDetailsPage);
            CoreInstallationStep_0_0_1.AddAllowedChildNode(EventsInstallationConstants.DocumentTypeAliases.EventsOverviewPage, EventsInstallationConstants.DocumentTypeAliases.EventsDetailsPage);
        }

    }
}
