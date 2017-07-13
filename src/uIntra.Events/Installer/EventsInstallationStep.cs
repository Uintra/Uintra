using uIntra.Core.Installer;
using Umbraco.Core;

namespace uIntra.Events.Installer
{
    public class EventsInstallationStep: IIntranetInstallationStep
    {
        public string PackageName => "uIntra.Events";
        public int Priority => 2;

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

            eventsOverviewPage = CoreInstallationStep.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);
            //TODO: Move static methods to service

            eventsOverviewPage.Name = EventsInstallationConstants.DocumentTypeNames.EventsOverviewPage;
            eventsOverviewPage.Alias = EventsInstallationConstants.DocumentTypeAliases.EventsOverviewPage;
            eventsOverviewPage.Icon = EventsInstallationConstants.DocumentTypeIcons.EventsOverviewPage;

            contentService.Save(eventsOverviewPage);
            CoreInstallationStep.AddAllowedChildNode(CoreInstallationConstants.DocumentTypeAliases.HomePage, EventsInstallationConstants.DocumentTypeAliases.EventsOverviewPage);
        }

        private void CreateEventsCreatePage()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var eventsCreatePage = contentService.GetContentType(EventsInstallationConstants.DocumentTypeAliases.EventsCreatePage);
            if (eventsCreatePage != null) return;

            eventsCreatePage = CoreInstallationStep.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);
            //TODO: Move static methods to service

            eventsCreatePage.Name = EventsInstallationConstants.DocumentTypeNames.EventsCreatePage;
            eventsCreatePage.Alias = EventsInstallationConstants.DocumentTypeAliases.EventsCreatePage;
            eventsCreatePage.Icon = EventsInstallationConstants.DocumentTypeIcons.EventsCreatePage;

            contentService.Save(eventsCreatePage);
            CoreInstallationStep.AddAllowedChildNode(EventsInstallationConstants.DocumentTypeAliases.EventsOverviewPage, EventsInstallationConstants.DocumentTypeAliases.EventsCreatePage);
        }
        private void CreateEventsEditPage()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var eventsEditPage = contentService.GetContentType(EventsInstallationConstants.DocumentTypeAliases.EventsEditPage);
            if (eventsEditPage != null) return;

            eventsEditPage = CoreInstallationStep.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);
            //TODO: Move static methods to service

            eventsEditPage.Name = EventsInstallationConstants.DocumentTypeNames.EventsEditPage;
            eventsEditPage.Alias = EventsInstallationConstants.DocumentTypeAliases.EventsEditPage;
            eventsEditPage.Icon = EventsInstallationConstants.DocumentTypeIcons.EventsEditPage;

            contentService.Save(eventsEditPage);
            CoreInstallationStep.AddAllowedChildNode(EventsInstallationConstants.DocumentTypeAliases.EventsOverviewPage, EventsInstallationConstants.DocumentTypeAliases.EventsEditPage);
        }
        private void CreateEventsDetailsPage()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var eventsDetailsPage = contentService.GetContentType(EventsInstallationConstants.DocumentTypeAliases.EventsDetailsPage);
            if (eventsDetailsPage != null) return;

            eventsDetailsPage = CoreInstallationStep.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);
            //TODO: Move static methods to service

            eventsDetailsPage.Name = EventsInstallationConstants.DocumentTypeNames.EventsDetailsPage;
            eventsDetailsPage.Alias = EventsInstallationConstants.DocumentTypeAliases.EventsDetailsPage;
            eventsDetailsPage.Icon = EventsInstallationConstants.DocumentTypeIcons.EventsDetailsPage;

            contentService.Save(eventsDetailsPage);
            CoreInstallationStep.AddAllowedChildNode(EventsInstallationConstants.DocumentTypeAliases.EventsOverviewPage, EventsInstallationConstants.DocumentTypeAliases.EventsDetailsPage);
        }

    }
}
