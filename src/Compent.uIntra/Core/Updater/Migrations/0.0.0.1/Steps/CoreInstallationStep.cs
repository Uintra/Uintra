using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Compent.uIntra.Core.Updater.Migrations._0._0._0._1.Steps.AggregateSubsteps;
using Newtonsoft.Json.Linq;
using uIntra.Core;
using uIntra.Core.Constants;
using uIntra.Core.Extensions;
using uIntra.Core.Installer;
using uIntra.Core.Media;
using uIntra.Core.Utils;
using Umbraco.Core;
using Umbraco.Core.Models;
using static Compent.uIntra.Core.Updater.ExecutionResult;
using static Compent.uIntra.Core.Updater.Migrations._0._0._0._1.Constants.CoreInstallationConstants;

namespace Compent.uIntra.Core.Updater.Migrations._0._0._0._1.Steps
{
    public class CoreInstallationStep : IMigrationStep
    {

        public ExecutionResult Execute()
        {
            CreateDocumentTypesFolders();
            CreateDataFolderDocumentType();
            CreateBasePageDocumentType();
            CreateDefaultGridDataType();
            CreateContentGridDataType();
            CreateBasePageWithGrid();
            CreateBasePageWithContentGrid();
            CreateGridPageLayoutTemplate();
            CreateHomePage();
            CreateErrorPage();
            CreateContentPage();

            AddImageCropperPreset();
            CreateLinksPickerDataType();
            AddIsDeletedProperty();
            CreateMediaFolderTypeDataType();
            AddFolderProperties();
            CreateDefaultFolders();
            AddIntranetUserIdProperty();
            return Success;
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        private void CreateDocumentTypesFolders()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;
            contentService.CreateContentTypeContainer(-1, DocumentTypesContainerNames.Compositions);
            contentService.CreateContentTypeContainer(-1, DocumentTypesContainerNames.DataContent);
            contentService.CreateContentTypeContainer(-1, DocumentTypesContainerNames.Folders);
            contentService.CreateContentTypeContainer(-1, DocumentTypesContainerNames.Pages);
        }

        private void CreateDataFolderDocumentType()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var dataFolderDocType = contentService.GetContentType(DocumentTypeAliases.DataFolder);
            if (dataFolderDocType != null) return;

            var folder = contentService.GetContentTypeContainers(DocumentTypesContainerNames.Folders, 1).First();
            dataFolderDocType = new ContentType(folder.Id)
            {
                Name = DocumentTypeNames.DataFolder,
                Alias = DocumentTypeAliases.DataFolder,
                AllowedAsRoot = true
            };

            contentService.Save(dataFolderDocType);
        }
        private void CreateBasePageDocumentType()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var basePageDocumentType = contentService.GetContentType(DocumentTypeAliases.BasePage);
            if (basePageDocumentType != null) return;

            var pagesFolder = contentService.GetContentTypeContainers(DocumentTypesContainerNames.Pages, 1).First();
            basePageDocumentType = new ContentType(pagesFolder.Id)
            {
                Name = DocumentTypeNames.BasePage,
                Alias = DocumentTypeAliases.BasePage
            };

            contentService.Save(basePageDocumentType);
        }

        private void CreateDefaultGridDataType()
        {
            var embeddedResourceFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.Core.Updater.Migrations._0._0._0._1.PreValues.DefaultGridCorePreValues.json";
            InstallationStepsHelper.CreateGrid(DataTypeNames.DefaultGrid, embeddedResourceFileName);
        }

        private void CreateContentGridDataType()
        {
            var embeddedResourceFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.Core.Updater.Migrations._0._0._0._1.PreValues.ContentGridCorePreValues.json";
            InstallationStepsHelper.CreateGrid(DataTypeNames.ContentGrid, embeddedResourceFileName);
        }

        private void CreateBasePageWithGrid()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var basePageDocumentType = contentService.GetContentType(DocumentTypeAliases.BasePageWithGrid);
            if (basePageDocumentType != null) return;

            var basePage = contentService.GetContentType(DocumentTypeAliases.BasePage);
            basePageDocumentType = new ContentType(basePage.Id)
            {
                Name = DocumentTypeNames.BasePageWithGrid,
                Alias = DocumentTypeAliases.BasePageWithGrid
            };

            basePageDocumentType.AddPropertyGroup(DataTypePropertyGroupNames.Content);
            basePageDocumentType.AddPropertyType(InstallationStepsHelper.GetGridPropertyType(DataTypeNames.DefaultGrid), DataTypePropertyGroupNames.Content);

            contentService.Save(basePageDocumentType);
        }

        private void CreateBasePageWithContentGrid()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var basePageDocumentType = contentService.GetContentType(DocumentTypeAliases.BasePageWithContentGrid);
            if (basePageDocumentType != null) return;

            var basePage = contentService.GetContentType(DocumentTypeAliases.BasePage);
            basePageDocumentType = new ContentType(basePage.Id)
            {
                Name = DocumentTypeNames.BasePageWithContentGrid,
                Alias = DocumentTypeAliases.BasePageWithContentGrid
            };

            basePageDocumentType.AddPropertyGroup(DataTypePropertyGroupNames.Content);
            basePageDocumentType.AddPropertyType(InstallationStepsHelper.GetGridPropertyType(DataTypeNames.ContentGrid), DataTypePropertyGroupNames.Content);

            contentService.Save(basePageDocumentType);
        }

        private void CreateHomePage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = DocumentTypeNames.HomePage,
                Alias = DocumentTypeAliases.HomePage,
                Icon = DocumentTypeIcons.HomePage
            };

            var homePage = InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
            homePage.AllowedAsRoot = true;

            var contentService = ApplicationContext.Current.Services.ContentTypeService;
            contentService.Save(homePage);
        }

        private void CreateErrorPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = DocumentTypeNames.ErrorPage,
                Alias = DocumentTypeAliases.ErrorPage,
                Icon = DocumentTypeIcons.ErrorPage,
                ParentAlias = DocumentTypeAliases.HomePage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateContentPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = DocumentTypeNames.ContentPage,
                Alias = DocumentTypeAliases.ContentPage,
                Icon = DocumentTypeIcons.ContentPage,
                ParentAlias = DocumentTypeAliases.HomePage
            };

            InstallationStepsHelper.CreatePageDocTypeWithContentGrid(createModel);
            InstallationStepsHelper.AddAllowedChildNode(DocumentTypeAliases.ContentPage, DocumentTypeAliases.ContentPage);
        }

        private void CreateGridPageLayoutTemplate()
        {
            var fileService = ApplicationContext.Current.Services.FileService;
            var alias = DocumentTypeAliases.GridPageLayoutTemplateAlias;
            var gridPageLayoutTemplate = fileService.GetTemplate(alias);
            if (gridPageLayoutTemplate != null) return;

            gridPageLayoutTemplate = new Template(alias, alias);

            var layoutEmbeddedResourceFileName = $"{ Assembly.GetExecutingAssembly().GetName().Name}.Core.Updater.Migrations._0._0._0._1.PreValues.GridPageLayout.cshtml";
            gridPageLayoutTemplate.Content = EmbeddedResourcesUtils.ReadResourceContent(layoutEmbeddedResourceFileName);

            fileService.SaveTemplate(gridPageLayoutTemplate);
        }

        private static void AddImageCropperPreset()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var imageCropperDataType = dataTypeService.GetDataTypeDefinitionByName(DataTypeNames.ImageCropper);
            var preValues = dataTypeService.GetPreValuesCollectionByDataTypeId(imageCropperDataType.Id);
            var preValuesDictionary = preValues.PreValuesAsDictionary;

            var preValuesArray = new JArray();

            if (preValuesDictionary.ContainsKey(DataTypePropertyPreValues.ImageCropperPresetsAlias))
            {
                preValuesArray = JArray.Parse(preValues.PreValuesAsDictionary[DataTypePropertyPreValues.ImageCropperPresetsAlias].Value);
                foreach (var child in preValuesArray.Children())
                {
                    if (child.Value<string>("alias") == DataTypePropertyPreValues.ImageCropperPresetDictionaryName)
                    {
                        return;
                    }
                }
            }

            var preset = new
            {
                alias = DataTypePropertyPreValues.ImageCropperPresetDictionaryName,
                height = DataTypePropertyPreValues.ImageCropperPresetHeigth,
                width = DataTypePropertyPreValues.ImageCropperPresetWidth
            };

            preValuesArray.Add(JObject.FromObject(preset));

            var newVal = preValuesArray.ToString();
            preValues.PreValuesAsDictionary.Add(DataTypePropertyPreValues.ImageCropperPresetsAlias, new PreValue(newVal));
            dataTypeService.SavePreValues(imageCropperDataType, preValues.PreValuesAsDictionary);
        }

        private void CreateLinksPickerDataType()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;

            var folderTypeDataType = dataTypeService.GetDataTypeDefinitionByName(DataTypeNames.LinksPicker);
            if (folderTypeDataType == null)
            {
                folderTypeDataType = new DataTypeDefinition(-1, DataTypePropertyEditors.LinksPicker)
                {
                    Name = DataTypeNames.LinksPicker
                };

                dataTypeService.Save(folderTypeDataType);
            }
        }

        private void AddIsDeletedProperty()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var contentTypeService = ApplicationContext.Current.Services.ContentTypeService;

            var imageType = contentTypeService.GetMediaType(UmbracoAliases.Media.ImageTypeAlias);
            var fileType = contentTypeService.GetMediaType(UmbracoAliases.Media.FileTypeAlias);

            var dataType = dataTypeService.GetDataTypeDefinitionByName(UmbracoAliases.Media.IsDeletedDataTypeDefinitionName);
            if (dataType == null)
            {
                dataType = new DataTypeDefinition("Umbraco.TrueFalse")
                {
                    Name = UmbracoAliases.Media.IsDeletedDataTypeDefinitionName
                };

                dataTypeService.Save(dataType);
            }

            var imageIsDeletedPropertyType = GetIsDeletedPropertyType(dataType);
            if (!imageType.PropertyTypeExists(imageIsDeletedPropertyType.Alias))
            {
                imageType.AddPropertyType(imageIsDeletedPropertyType);
                contentTypeService.Save(imageType);
            }

            var fileIsDeletedPropertyType = GetIsDeletedPropertyType(dataType);
            if (!fileType.PropertyTypeExists(fileIsDeletedPropertyType.Alias))
            {
                fileType.AddPropertyType(fileIsDeletedPropertyType);
                contentTypeService.Save(fileType);
            }
        }

        private PropertyType GetIsDeletedPropertyType(IDataTypeDefinition dataType)
        {
            return new PropertyType(dataType)
            {
                Name = "Is deleted",
                Alias = UmbracoAliases.Media.IsDeletedPropertyTypeAlias
            };
        }

        private void CreateMediaFolderTypeDataType()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;

            var folderTypeDataType = dataTypeService.GetDataTypeDefinitionByName(FolderConstants.DataTypeName);
            if (folderTypeDataType == null)
            {
                folderTypeDataType = new DataTypeDefinition(-1, UmbracoAliases.EnumDropdownList)
                {
                    Name = FolderConstants.DataTypeName
                };

                var preValues = new Dictionary<string, PreValue>
                {
                    { FolderConstants.PreValueAssemblyAlias, new PreValue(FolderConstants.EnumAssemblyDll)},
                    { FolderConstants.PreValueEnumAlias, new PreValue(typeof(MediaFolderTypeEnum).FullName)}
                };
                dataTypeService.SaveDataTypeAndPreValues(folderTypeDataType, preValues);
            }
        }

        private void AddFolderProperties()
        {
            var contentTypeService = ApplicationContext.Current.Services.ContentTypeService;
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;

            var folderType = contentTypeService.GetMediaType(UmbracoAliases.Media.FolderTypeAlias);
            var folderTypeDataType = dataTypeService.GetDataTypeDefinitionByName(FolderConstants.DataTypeName);

            var folderTypePropertyType = new PropertyType(folderTypeDataType)
            {
                Name = FolderConstants.FolderTypePropertyTypeName,
                Alias = FolderConstants.FolderTypePropertyTypeAlias
            };

            if (!folderType.PropertyTypeExists(folderTypePropertyType.Alias))
            {
                folderType.AddPropertyType(folderTypePropertyType);
            }

            var allowedMediaExtensionsPropertyType = new PropertyType("Umbraco.Textbox", DataTypeDatabaseType.Nvarchar)
            {
                Alias = FolderConstants.AllowedMediaExtensionsPropertyTypeAlias,
                Name = FolderConstants.AllowedMediaExtensionsPropertyTypeName
            };

            if (!folderType.PropertyTypeExists(allowedMediaExtensionsPropertyType.Alias))
            {
                folderType.AddPropertyType(allowedMediaExtensionsPropertyType);
            }

            contentTypeService.Save(folderType);
        }

        private void CreateDefaultFolders()
        {
            var mediaService = ApplicationContext.Current.Services.MediaService;
            var folderTypes = Enum.GetValues(typeof(MediaFolderTypeEnum)).Cast<MediaFolderTypeEnum>();
            var rootFolders = mediaService.GetRootMedia().Where(f => f.ContentType.Alias.Equals(UmbracoAliases.Media.FolderTypeAlias));
            foreach (var folderType in folderTypes)
            {
                var folderName = folderType.GetAttribute<DisplayAttribute>().Name;
                var folderByName = rootFolders.Where(m => m.Name.Equals(folderName));
                var folderByType = rootFolders.Where(m => m.GetValue<MediaFolderTypeEnum>(FolderConstants.FolderTypePropertyTypeAlias) == folderType);

                if (folderByName.Any() || folderByType.Any())
                {
                    continue;
                }

                var mediaFolder = mediaService.CreateMedia(folderName, -1, UmbracoAliases.Media.FolderTypeAlias);
                mediaFolder.SetValue(FolderConstants.FolderTypePropertyTypeAlias, folderType.ToString());
                mediaService.Save(mediaFolder);
            }
        }

        private void AddIntranetUserIdProperty()
        {
            var contentTypeService = ApplicationContext.Current.Services.ContentTypeService;

            var imageType = contentTypeService.GetMediaType(UmbracoAliases.Media.ImageTypeAlias);
            var fileType = contentTypeService.GetMediaType(UmbracoAliases.Media.FileTypeAlias);

            var imageIntranetUserId = GetIntranetUserIdPropertyType();
            if (!imageType.PropertyTypeExists(imageIntranetUserId.Alias))
            {
                imageType.AddPropertyType(imageIntranetUserId);
                contentTypeService.Save(imageType);
            }

            var fileIntranetUserId = GetIntranetUserIdPropertyType();
            if (!fileType.PropertyTypeExists(fileIntranetUserId.Alias))
            {
                fileType.AddPropertyType(fileIntranetUserId);
                contentTypeService.Save(fileType);
            }
        }

        private PropertyType GetIntranetUserIdPropertyType()
        {
            return new PropertyType("Umbraco.NoEdit", DataTypeDatabaseType.Nvarchar)
            {
                Name = "Intranet user id",
                Alias = IntranetConstants.IntranetCreatorId
            };
        }
    }
}
