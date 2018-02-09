using System.Linq;
using Uintra.Core.Installer;
using Umbraco.Core;
using Umbraco.Core.Models;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using static Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants.NavigationInstallationConstants;
using static Compent.Uintra.Core.Updater.ExecutionResult;

namespace Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Steps
{
    public class NavigationInstallationStep: IMigrationStep
    {

        public ExecutionResult Execute()
        {
            CreateSystemLinkFolder();
            CreateNavigationTrueFalseDataTypes();
            CreateNavigationComposition();
            CreateHomeNavigationComposition();
            CreateLinksPickerDataType();
            CreateSystemLinkDocumentType();
            return Success;
        }

        public void Undo()
        {
            throw new System.NotImplementedException();
        }

        private void CreateSystemLinkFolder()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var dataFolderDocType = contentService.GetContentType(DocumentTypeAliases.SystemLinkFolder);
            if (dataFolderDocType != null) return;

            var folder = contentService.GetContentTypeContainers(CoreInstallationConstants.DocumentTypesContainerNames.Folders, 1).First();
            dataFolderDocType = new ContentType(folder.Id)
            {
                Name = DocumentTypeNames.SystemLinkFolder,
                Alias = DocumentTypeAliases.SystemLinkFolder
            };

            contentService.Save(dataFolderDocType);

            InstallationStepsHelper.AddAllowedChildNode(CoreInstallationConstants.DocumentTypeAliases.DataFolder, DocumentTypeAliases.SystemLinkFolder);
        }

        private void CreateNavigationComposition()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;

            var compositionFolder = contentService.GetContentTypeContainers(CoreInstallationConstants.DocumentTypesContainerNames.Compositions, 1).First();

            var navigationCompositionType = contentService.GetContentType(DocumentTypeAliases.NavigationComposition);
            if (navigationCompositionType != null) return;

            navigationCompositionType = new ContentType(compositionFolder.Id)
            {
                Name = DocumentTypeNames.NavigationComposition,
                Alias = DocumentTypeAliases.NavigationComposition
            };

            navigationCompositionType.AddPropertyGroup(DocumentTypeTabNames.Navigation);

            var navigationNameProperty = new PropertyType("Umbraco.Textbox", DataTypeDatabaseType.Nvarchar)
            {
                Name = DocumentTypePropertyNames.NavigationName,
                Alias = DocumentTypePropertyAliases.NavigationName,
                Mandatory = true
            };

            var isHideFromLeftNav = dataTypeService.GetDataTypeDefinitionByName(DataTypeNames.IsHideFromLeftNavigation);
            var isHideFromLeftProperty = new PropertyType(isHideFromLeftNav)
            {
                Name = DocumentTypePropertyNames.HideFromLeftNavigation,
                Alias = DocumentTypePropertyAliases.IsHideFromLeftNavigation
            };

            var isHideFromSubNav = dataTypeService.GetDataTypeDefinitionByName(DataTypeNames.IsHideFromSubNavigation);
            var isHideFromSubProperty = new PropertyType(isHideFromSubNav)
            {
                Name = DocumentTypePropertyNames.HideFromSubNavigation,
                Alias = DocumentTypePropertyAliases.IsHideFromSubNavigation
            };

            navigationCompositionType.AddPropertyType(navigationNameProperty, DocumentTypeTabNames.Navigation);
            navigationCompositionType.AddPropertyType(isHideFromLeftProperty, DocumentTypeTabNames.Navigation);
            navigationCompositionType.AddPropertyType(isHideFromSubProperty, DocumentTypeTabNames.Navigation);

            contentService.Save(navigationCompositionType);
        }

        private void CreateSystemLinkDocumentType()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;

            var compositionFolder = contentService.GetContentTypeContainers(CoreInstallationConstants.DocumentTypesContainerNames.DataContent, 1).First();

            var navigationCompositionType = contentService.GetContentType(DocumentTypeAliases.SystemLink);
            if (navigationCompositionType != null) return;

            navigationCompositionType = new ContentType(compositionFolder.Id)
            {
                Name = DocumentTypeNames.SystemLink,
                Alias = DocumentTypeAliases.SystemLink
            };

            navigationCompositionType.AddPropertyGroup(DocumentTypeTabNames.Links);

            var linksGroupTitleProperty = new PropertyType("Umbraco.Textbox", DataTypeDatabaseType.Nvarchar)
            {
                Name = DocumentTypePropertyNames.LinksGroupTitle,
                Alias = DocumentTypePropertyAliases.LinksGroupTitle
            };
            var sortProperty = new PropertyType("Umbraco.Integer", DataTypeDatabaseType.Integer)
            {
                Name = DocumentTypePropertyNames.Sort,
                Alias = DocumentTypePropertyAliases.Sort
            };

            var linksPicker = dataTypeService.GetDataTypeDefinitionByName(DataTypeNames.LinksPicker);
            var linksProperty = new PropertyType(linksPicker)
            {
                Name = DocumentTypePropertyNames.Links,
                Alias = DocumentTypePropertyAliases.Links
            };

            navigationCompositionType.AddPropertyType(linksGroupTitleProperty, DocumentTypeTabNames.Links);
            navigationCompositionType.AddPropertyType(sortProperty, DocumentTypeTabNames.Navigation);
            navigationCompositionType.AddPropertyType(linksProperty, DocumentTypeTabNames.Navigation);

            contentService.Save(navigationCompositionType);
            InstallationStepsHelper.AddAllowedChildNode(DocumentTypeAliases.SystemLinkFolder, DocumentTypeAliases.SystemLink);

        }

        private void CreateLinksPickerDataType()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;

            var dataType = dataTypeService.GetDataTypeDefinitionByName(DataTypeNames.LinksPicker);
            if (dataType != null) return;

            dataType = new DataTypeDefinition(DataTypePropertyEditors.LinksPicker)
            {
                Name = DataTypeNames.LinksPicker
            };

            dataTypeService.Save(dataType);
        }

        private void CreateHomeNavigationComposition()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;

            var compositionFolder = contentService.GetContentTypeContainers(CoreInstallationConstants.DocumentTypesContainerNames.Compositions, 1).First();

            var systemLink = contentService.GetContentType(DocumentTypeAliases.HomeNavigationComposition);
            if (systemLink != null) return;

            systemLink = new ContentType(compositionFolder.Id)
            {
                Name = DocumentTypeNames.HomeNavigationComposition,
                Alias = DocumentTypeAliases.HomeNavigationComposition
            };

            systemLink.AddPropertyGroup(DocumentTypeTabNames.Navigation);

            var showInHome = dataTypeService.GetDataTypeDefinitionByName(DataTypeNames.IsShowInHomeNavigationTrueFalse);
            var showInHomeProperty = new PropertyType(showInHome)
            {
                Name = DocumentTypePropertyNames.IsShowInHomeNavigation,
                Alias = DocumentTypePropertyAliases.IsShowInHomeNavigation
            };

            systemLink.AddPropertyType(showInHomeProperty, DocumentTypeTabNames.Navigation);

            contentService.Save(systemLink);
        }

        private void CreateNavigationTrueFalseDataTypes()
        {
            InstallationStepsHelper.CreateTrueFalseDataType(DataTypeNames.IsShowInHomeNavigationTrueFalse);
            InstallationStepsHelper.CreateTrueFalseDataType(DataTypeNames.IsHideFromLeftNavigation);
            InstallationStepsHelper.CreateTrueFalseDataType(DataTypeNames.IsHideFromSubNavigation);
        }
    }
}
