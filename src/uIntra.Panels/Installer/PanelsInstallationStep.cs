using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using Newtonsoft.Json.Linq;
using uIntra.Core.Installer;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace uIntra.Panels.Installer
{
    public class PanelsInstallationStep : IIntranetInstallationStep
    {
        public string PackageName => "uIntra.Panels";
        public int Priority => 2;

        public void Execute()
        {
            CreatePanelPickerDataType();
            CreateGlobalPanelFolder();
            CreatePanelDocumentType();
        }

        private void CreatePanelPickerDataType()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var panelPickerDataType = dataTypeService.GetDataTypeDefinitionByName(PanelsInstallationConstants.DataTypeNames.PanelPicker);
            if (panelPickerDataType != null) return;

            var configPath = HostingEnvironment.MapPath("~/Installer/PreValues/PanelPickerPreValues.json");
            var jsonPrevalues = JObject.Parse(System.IO.File.ReadAllText(configPath));

            panelPickerDataType = new DataTypeDefinition(-1, PanelsInstallationConstants.DataTypePropertyEditors.PanelPicker)
            {
                Name = PanelsInstallationConstants.DataTypeNames.PanelPicker
            };
            var preValues = new Dictionary<string, PreValue>
            {
                { PanelsInstallationConstants.DataTypePreValueAliases.PanelPickerConfig, new PreValue(jsonPrevalues.ToString())}
            };
            dataTypeService.SaveDataTypeAndPreValues(panelPickerDataType, preValues);
        }

        private void CreatePanelDocumentType()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var panelDocumentType = contentService.GetContentType(PanelsInstallationConstants.DocumentTypeNames.Panel);

            if (panelDocumentType != null) return;

            panelDocumentType = new ContentType(-1)
            {
                Name = PanelsInstallationConstants.DocumentTypeNames.Panel,
                Alias = PanelsInstallationConstants.DocumentTypeAliases.Panel
            };

            panelDocumentType.AddPropertyGroup("Panel");
            var panelPickerDataType = dataTypeService.GetDataTypeDefinitionByName(PanelsInstallationConstants.DataTypeNames.PanelPicker);
            var panelPickerProperty = new PropertyType(panelPickerDataType)
            {
                Name = PanelsInstallationConstants.DocumentTypePropertyNames.PanelConfig,
                Alias = PanelsInstallationConstants.DocumentTypePropertyAliases.PanelConfig
            };
            panelDocumentType.AddPropertyType(panelPickerProperty, "Panel");

            contentService.Save(panelDocumentType);
            CoreInstallationStep.AddAllowedChildNode(PanelsInstallationConstants.DocumentTypeAliases.GlobalPanelFolder, PanelsInstallationConstants.DocumentTypeAliases.Panel);
        }

        private void CreateGlobalPanelFolder()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var dataFolderDocType = contentService.GetContentType(PanelsInstallationConstants.DocumentTypeAliases.GlobalPanelFolder);
            if (dataFolderDocType != null) return;

            var folder = contentService.GetContentTypeContainers(CoreInstallationConstants.DocumentTypesContainerNames.Folders, 1).First();
            dataFolderDocType = new ContentType(folder.Id)
            {
                Name = PanelsInstallationConstants.DocumentTypeNames.GlobalPanelFolder,
                Alias = PanelsInstallationConstants.DocumentTypeAliases.GlobalPanelFolder
            };

            contentService.Save(dataFolderDocType);

            CoreInstallationStep.AddAllowedChildNode(CoreInstallationConstants.DocumentTypeAliases.DataFolder, PanelsInstallationConstants.DocumentTypeAliases.GlobalPanelFolder);
        }
    }
}
