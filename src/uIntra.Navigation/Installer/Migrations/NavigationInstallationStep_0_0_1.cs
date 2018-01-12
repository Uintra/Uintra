using System.Linq;
using uIntra.Core.Installer;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace uIntra.Navigation.Installer.Migrations
{
    public class NavigationInstallationStep_0_0_1 : IIntranetInstallationStep
    {
        public string PackageName => "uIntra.Navigation";
        public int Priority => 2;
        public string Version => InstallationVersionConstrants.Version_0_0_1;

        public void Execute()
        {
            CreateSystemLinkFolder();
            CreateNavigationTrueFalseDataTypes();
            CreateNavigationComposition();
            CreateHomeNavigationComposition();
            CreateLinksPickerDataType();
            CreateSystemLinkDocumentType();
        }

        private void CreateSystemLinkFolder()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var dataFolderDocType = contentService.GetContentType(NavigationInstallationConstants.DocumentTypeAliases.SystemLinkFolder);
            if (dataFolderDocType != null) return;

            var folder = contentService.GetContentTypeContainers(CoreInstallationConstants.DocumentTypesContainerNames.Folders, 1).First();
            dataFolderDocType = new ContentType(folder.Id)
            {
                Name = NavigationInstallationConstants.DocumentTypeNames.SystemLinkFolder,
                Alias = NavigationInstallationConstants.DocumentTypeAliases.SystemLinkFolder
            };

            contentService.Save(dataFolderDocType);

            InstallationStepsHelper.AddAllowedChildNode(CoreInstallationConstants.DocumentTypeAliases.DataFolder, NavigationInstallationConstants.DocumentTypeAliases.SystemLinkFolder);
        }

        private void CreateNavigationComposition()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;

            var compositionFolder = contentService.GetContentTypeContainers(CoreInstallationConstants.DocumentTypesContainerNames.Compositions, 1).First();

            var navigationCompositionType = contentService.GetContentType(NavigationInstallationConstants.DocumentTypeAliases.NavigationComposition);
            if (navigationCompositionType != null) return;

            navigationCompositionType = new ContentType(compositionFolder.Id)
            {
                Name = NavigationInstallationConstants.DocumentTypeNames.NavigationComposition,
                Alias = NavigationInstallationConstants.DocumentTypeAliases.NavigationComposition
            };

            navigationCompositionType.AddPropertyGroup(NavigationInstallationConstants.DocumentTypeTabNames.Navigation);

            var navigationNameProperty = new PropertyType("Umbraco.Textbox", DataTypeDatabaseType.Nvarchar)
            {
                Name = NavigationInstallationConstants.DocumentTypePropertyNames.NavigationName,
                Alias = NavigationInstallationConstants.DocumentTypePropertyAliases.NavigationName,
                Mandatory = true
            };

            var isHideFromLeftNav = dataTypeService.GetDataTypeDefinitionByName(NavigationInstallationConstants.DataTypeNames.IsHideFromLeftNavigation);
            var isHideFromLeftProperty = new PropertyType(isHideFromLeftNav)
            {
                Name = NavigationInstallationConstants.DocumentTypePropertyNames.HideFromLeftNavigation,
                Alias = NavigationInstallationConstants.DocumentTypePropertyAliases.IsHideFromLeftNavigation
            };

            var isHideFromSubNav = dataTypeService.GetDataTypeDefinitionByName(NavigationInstallationConstants.DataTypeNames.IsHideFromSubNavigation);
            var isHideFromSubProperty = new PropertyType(isHideFromSubNav)
            {
                Name = NavigationInstallationConstants.DocumentTypePropertyNames.HideFromSubNavigation,
                Alias = NavigationInstallationConstants.DocumentTypePropertyAliases.IsHideFromSubNavigation
            };

            navigationCompositionType.AddPropertyType(navigationNameProperty, NavigationInstallationConstants.DocumentTypeTabNames.Navigation);
            navigationCompositionType.AddPropertyType(isHideFromLeftProperty, NavigationInstallationConstants.DocumentTypeTabNames.Navigation);
            navigationCompositionType.AddPropertyType(isHideFromSubProperty, NavigationInstallationConstants.DocumentTypeTabNames.Navigation);

            contentService.Save(navigationCompositionType);
        }

        private void CreateSystemLinkDocumentType()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;

            var compositionFolder = contentService.GetContentTypeContainers(CoreInstallationConstants.DocumentTypesContainerNames.DataContent, 1).First();

            var navigationCompositionType = contentService.GetContentType(NavigationInstallationConstants.DocumentTypeAliases.SystemLink);
            if (navigationCompositionType != null) return;

            navigationCompositionType = new ContentType(compositionFolder.Id)
            {
                Name = NavigationInstallationConstants.DocumentTypeNames.SystemLink,
                Alias = NavigationInstallationConstants.DocumentTypeAliases.SystemLink
            };

            navigationCompositionType.AddPropertyGroup(NavigationInstallationConstants.DocumentTypeTabNames.Links);

            var linksGroupTitleProperty = new PropertyType("Umbraco.Textbox", DataTypeDatabaseType.Nvarchar)
            {
                Name = NavigationInstallationConstants.DocumentTypePropertyNames.LinksGroupTitle,
                Alias = NavigationInstallationConstants.DocumentTypePropertyAliases.LinksGroupTitle
            };
            var sortProperty = new PropertyType("Umbraco.Integer", DataTypeDatabaseType.Integer)
            {
                Name = NavigationInstallationConstants.DocumentTypePropertyNames.Sort,
                Alias = NavigationInstallationConstants.DocumentTypePropertyAliases.Sort
            };

            var linksPicker = dataTypeService.GetDataTypeDefinitionByName(NavigationInstallationConstants.DataTypeNames.LinksPicker);
            var linksProperty = new PropertyType(linksPicker)
            {
                Name = NavigationInstallationConstants.DocumentTypePropertyNames.Links,
                Alias = NavigationInstallationConstants.DocumentTypePropertyAliases.Links
            };

            navigationCompositionType.AddPropertyType(linksGroupTitleProperty, NavigationInstallationConstants.DocumentTypeTabNames.Links);
            navigationCompositionType.AddPropertyType(sortProperty, NavigationInstallationConstants.DocumentTypeTabNames.Navigation);
            navigationCompositionType.AddPropertyType(linksProperty, NavigationInstallationConstants.DocumentTypeTabNames.Navigation);

            contentService.Save(navigationCompositionType);
            InstallationStepsHelper.AddAllowedChildNode(NavigationInstallationConstants.DocumentTypeAliases.SystemLinkFolder, NavigationInstallationConstants.DocumentTypeAliases.SystemLink);

        }

        private void CreateLinksPickerDataType()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;

            var dataType = dataTypeService.GetDataTypeDefinitionByName(NavigationInstallationConstants.DataTypeNames.LinksPicker);
            if (dataType != null) return;

            dataType = new DataTypeDefinition(NavigationInstallationConstants.DataTypePropertyEditors.LinksPicker)
            {
                Name = NavigationInstallationConstants.DataTypeNames.LinksPicker
            };

            dataTypeService.Save(dataType);
        }

        private void CreateHomeNavigationComposition()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;

            var compositionFolder = contentService.GetContentTypeContainers(CoreInstallationConstants.DocumentTypesContainerNames.Compositions, 1).First();

            var systemLink = contentService.GetContentType(NavigationInstallationConstants.DocumentTypeAliases.HomeNavigationComposition);
            if (systemLink != null) return;

            systemLink = new ContentType(compositionFolder.Id)
            {
                Name = NavigationInstallationConstants.DocumentTypeNames.HomeNavigationComposition,
                Alias = NavigationInstallationConstants.DocumentTypeAliases.HomeNavigationComposition
            };

            systemLink.AddPropertyGroup(NavigationInstallationConstants.DocumentTypeTabNames.Navigation);

            var showInHome = dataTypeService.GetDataTypeDefinitionByName(NavigationInstallationConstants.DataTypeNames.IsShowInHomeNavigationTrueFalse);
            var showInHomeProperty = new PropertyType(showInHome)
            {
                Name = NavigationInstallationConstants.DocumentTypePropertyNames.IsShowInHomeNavigation,
                Alias = NavigationInstallationConstants.DocumentTypePropertyAliases.IsShowInHomeNavigation
            };

            systemLink.AddPropertyType(showInHomeProperty, NavigationInstallationConstants.DocumentTypeTabNames.Navigation);

            contentService.Save(systemLink);
        }

        private void CreateNavigationTrueFalseDataTypes()
        {
            InstallationStepsHelper.CreateTrueFalseDataType(NavigationInstallationConstants.DataTypeNames.IsShowInHomeNavigationTrueFalse);
            InstallationStepsHelper.CreateTrueFalseDataType(NavigationInstallationConstants.DataTypeNames.IsHideFromLeftNavigation);
            InstallationStepsHelper.CreateTrueFalseDataType(NavigationInstallationConstants.DataTypeNames.IsHideFromSubNavigation);
        }
    }
}
