using Compent.Uintra.Core.Updater.Migrations._0._0._0._1;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Models;
using static Uintra.Core.OpenGraph.Constants.OpenGraphConstants;

namespace Compent.Uintra.Core.Updater.Migrations._1._4.Steps
{
    public class AddOpenGraphSettings : IMigrationStep
    {
        public ExecutionResult Execute()
        {
            CreateOpenGraphComposition();
            InheritOpenGraphComposition();
            return ExecutionResult.Success;
        }

        public void Undo()
        {
            
        }

        private void InheritOpenGraphComposition()
        {
            InstallationStepsHelper.InheritCompositionForPage(CoreInstallationConstants.DocumentTypeAliases.ContentPage,
                DocumentType.Alias);
        }

        private void CreateOpenGraphComposition()
        {
            var services = ApplicationContext.Current.Services;
            var contentService = services.ContentTypeService;
            var dataTypeService = services.DataTypeService;

            var compositionFolder = contentService.GetContentTypeContainers(
                CoreInstallationConstants.DocumentTypesContainerNames.Compositions, 1).First();

            var openGraphCompositionType = contentService.GetContentType(DocumentType.Alias);
            if (openGraphCompositionType != null) return;

            openGraphCompositionType = new ContentType(compositionFolder.Id)
            {
                Name = DocumentType.Name,
                Alias = DocumentType.Alias
            };

            openGraphCompositionType.AddPropertyGroup(DocumentType.TabName);

            var titleProperty = new PropertyType("Umbraco.Textbox", DataTypeDatabaseType.Nvarchar)
            {
                Name = Properties.TitleName,
                Alias = Properties.TitleAlias
            };

            var descriptionProperty = new PropertyType("Umbraco.TextboxMultiple", DataTypeDatabaseType.Nvarchar)
            {
                Name = Properties.DescriptionName,
                Alias = Properties.DescriptionAlias
            };

            var mediaPickerDataType = dataTypeService.GetDataTypeDefinitionByName(MediaPickerDataTypeName);
            if (mediaPickerDataType == null)
            {
                mediaPickerDataType = new DataTypeDefinition("Umbraco.MediaPicker2")
                {
                    Name = MediaPickerDataTypeName
                };
                dataTypeService.Save(mediaPickerDataType);
                mediaPickerDataType = dataTypeService.GetDataTypeDefinitionByName(MediaPickerDataTypeName);
            }
            var imageProperty = new PropertyType(mediaPickerDataType)
            {
                Name = Properties.ImageName,
                Alias = Properties.ImageAlias
            };

            openGraphCompositionType.AddPropertyType(titleProperty, DocumentType.TabName);
            openGraphCompositionType.AddPropertyType(descriptionProperty, DocumentType.TabName);
            openGraphCompositionType.AddPropertyType(imageProperty, DocumentType.TabName);


            var tab = openGraphCompositionType.PropertyGroups.First();
            tab.SortOrder = 20;

            contentService.Save(openGraphCompositionType);
        }

    }
}