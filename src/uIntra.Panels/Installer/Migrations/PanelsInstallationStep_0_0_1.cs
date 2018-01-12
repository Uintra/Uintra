using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using uIntra.Core.Installer;
using uIntra.Core.Utils;
using Umbraco.Core;
using Umbraco.Core.Models;
using static uIntra.Panels.Installer.PanelsInstallationConstants;

namespace uIntra.Panels.Installer.Migrations
{
    public class PanelsInstallationStep : IIntranetInstallationStep
    {
        public string PackageName => "uIntra.Panels";
        public int Priority => 2;
        public string Version => InstallationVersionConstrants.Version_0_0_1;

        public void Execute()
        {
            CreatePanelPickerDataType();
            CreateGlobalPanelFolder();
            CreatePanelDocumentType();
        }

        private void CreatePanelPickerDataType()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var panelPickerDataType = dataTypeService.GetDataTypeDefinitionByName(DataTypeNames.PanelPicker);
            if (panelPickerDataType != null) return;

            var jsonValue = EmbeddedResourcesUtils.ReadResourceContent("uIntra.Panels.Installer.PreValues.PanelPickerPreValues.json");
            var jsonPrevalues = JObject.Parse(jsonValue);

            panelPickerDataType = new DataTypeDefinition(-1, DataTypePropertyEditors.PanelPicker)
            {
                Name = DataTypeNames.PanelPicker
            };
            var preValues = new Dictionary<string, PreValue>
            {
                { DataTypePreValueAliases.PanelPickerConfig, new PreValue(jsonPrevalues.ToString())}
            };
            dataTypeService.SaveDataTypeAndPreValues(panelPickerDataType, preValues);
        }

        private void CreatePanelDocumentType()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var panelDocumentType = contentService.GetContentType(DocumentTypeNames.Panel);

            if (panelDocumentType != null) return;

            panelDocumentType = new ContentType(-1)
            {
                Name = DocumentTypeNames.Panel,
                Alias = DocumentTypeAliases.Panel,
                Icon = DocumentTypeIcons.Panel
            };

            panelDocumentType.AddPropertyGroup("Panel");
            var panelPickerDataType = dataTypeService.GetDataTypeDefinitionByName(DataTypeNames.PanelPicker);
            var panelPickerProperty = new PropertyType(panelPickerDataType)
            {
                Name = DocumentTypePropertyNames.PanelConfig,
                Alias = DocumentTypePropertyAliases.PanelConfig,
            };
            panelDocumentType.AddPropertyType(panelPickerProperty, "Panel");

            contentService.Save(panelDocumentType);
            InstallationStepsHelper.AddAllowedChildNode(DocumentTypeAliases.GlobalPanelFolder, DocumentTypeAliases.Panel);
        }

        private void CreateGlobalPanelFolder()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var dataFolderDocType = contentService.GetContentType(DocumentTypeAliases.GlobalPanelFolder);
            if (dataFolderDocType != null) return;

            var folder = contentService.GetContentTypeContainers(CoreInstallationConstants.DocumentTypesContainerNames.Folders, 1).First();
            dataFolderDocType = new ContentType(folder.Id)
            {
                Name = DocumentTypeNames.GlobalPanelFolder,
                Alias = DocumentTypeAliases.GlobalPanelFolder
            };

            contentService.Save(dataFolderDocType);

            InstallationStepsHelper.AddAllowedChildNode(CoreInstallationConstants.DocumentTypeAliases.DataFolder, DocumentTypeAliases.GlobalPanelFolder);
        }
    }
}
