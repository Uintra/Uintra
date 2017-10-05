using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using uIntra.Core.Constants;
using uIntra.Core.Extentions;
using uIntra.Core.Media;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace uIntra.Core.Installer.Migrations
{
    public class CoreInstallationStep_0_0_1 : IIntranetInstallationStep
    {
        public string PackageName => "uIntra.Core";
        public int Priority => 0;
        public string Version => InstallationVersionConstrants.Version_0_0_1;

        public void Execute()
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
        }

        private void CreateDocumentTypesFolders()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;
            contentService.CreateContentTypeContainer(-1, CoreInstallationConstants.DocumentTypesContainerNames.Compositions);
            contentService.CreateContentTypeContainer(-1, CoreInstallationConstants.DocumentTypesContainerNames.DataContent);
            contentService.CreateContentTypeContainer(-1, CoreInstallationConstants.DocumentTypesContainerNames.Folders);
            contentService.CreateContentTypeContainer(-1, CoreInstallationConstants.DocumentTypesContainerNames.Pages);
        }

        private void CreateDataFolderDocumentType()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var dataFolderDocType = contentService.GetContentType(CoreInstallationConstants.DocumentTypeAliases.DataFolder);
            if (dataFolderDocType != null) return;

            var folder = contentService.GetContentTypeContainers(CoreInstallationConstants.DocumentTypesContainerNames.Folders, 1).First();
            dataFolderDocType = new ContentType(folder.Id)
            {
                Name = CoreInstallationConstants.DocumentTypeNames.DataFolder,
                Alias = CoreInstallationConstants.DocumentTypeAliases.DataFolder,
                AllowedAsRoot = true
            };

            contentService.Save(dataFolderDocType);
        }
        private void CreateBasePageDocumentType()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var basePageDocumentType = contentService.GetContentType(CoreInstallationConstants.DocumentTypeAliases.BasePage);
            if (basePageDocumentType != null) return;

            var pagesFolder = contentService.GetContentTypeContainers(CoreInstallationConstants.DocumentTypesContainerNames.Pages, 1).First();
            basePageDocumentType = new ContentType(pagesFolder.Id)
            {
                Name = CoreInstallationConstants.DocumentTypeNames.BasePage,
                Alias = CoreInstallationConstants.DocumentTypeAliases.BasePage
            };

            contentService.Save(basePageDocumentType);
        }

        private void CreateDefaultGridDataType()
        {
            var embeddedResourceFileName = "uIntra.Core.Installer.PreValues.DefaultGridPreValues.json";
            CreateGrid(CoreInstallationConstants.DataTypeNames.DefaultGrid, embeddedResourceFileName);
        }
        private void CreateContentGridDataType()
        {
            var embeddedResourceFileName = "uIntra.Core.Installer.PreValues.ContentGridPreValues.json";
            CreateGrid(CoreInstallationConstants.DataTypeNames.ContentGrid, embeddedResourceFileName);
        }

        public static void CreateGrid(string dataTypeName, string gridEmbeddedResourceFileName)
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var defaultGridDataType = dataTypeService.GetDataTypeDefinitionByName(dataTypeName);
            if (defaultGridDataType != null) return;

            var gridJson = GetEmbeddedResourceValue(gridEmbeddedResourceFileName);

            var jsonPrevalues = JObject.Parse(gridJson);
            var preValueItemsAlias = CoreInstallationConstants.DataTypePropertyPreValues.DefaultGridItems;
            var preValueRteAlias = CoreInstallationConstants.DataTypePropertyPreValues.DefaultGridRte;
            defaultGridDataType = new DataTypeDefinition(-1, "Umbraco.Grid")
            {
                Name = dataTypeName
            };
            var preValues = new Dictionary<string, PreValue>
            {
                { preValueItemsAlias, new PreValue(jsonPrevalues.Property(preValueItemsAlias).Value.ToString())},
                { preValueRteAlias, new PreValue(jsonPrevalues.Property(preValueRteAlias).Value.ToString())}
            };
            dataTypeService.SaveDataTypeAndPreValues(defaultGridDataType, preValues);
        }

        private void CreateBasePageWithGrid()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var basePageDocumentType = contentService.GetContentType(CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);
            if (basePageDocumentType != null) return;

            var basePage = contentService.GetContentType(CoreInstallationConstants.DocumentTypeAliases.BasePage);
            basePageDocumentType = new ContentType(basePage.Id)
            {
                Name = CoreInstallationConstants.DocumentTypeNames.BasePageWithGrid,
                Alias = CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid
            };

            basePageDocumentType.AddPropertyGroup(CoreInstallationConstants.DataTypePropertyGroupNames.Content);
            basePageDocumentType.AddPropertyType(GetGridPropertyType(CoreInstallationConstants.DataTypeNames.DefaultGrid), CoreInstallationConstants.DataTypePropertyGroupNames.Content);

            contentService.Save(basePageDocumentType);
        }

        private void CreateBasePageWithContentGrid()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var basePageDocumentType = contentService.GetContentType(CoreInstallationConstants.DocumentTypeAliases.BasePageWithContentGrid);
            if (basePageDocumentType != null) return;

            var basePage = contentService.GetContentType(CoreInstallationConstants.DocumentTypeAliases.BasePage);
            basePageDocumentType = new ContentType(basePage.Id)
            {
                Name = CoreInstallationConstants.DocumentTypeNames.BasePageWithContentGrid,
                Alias = CoreInstallationConstants.DocumentTypeAliases.BasePageWithContentGrid
            };

            basePageDocumentType.AddPropertyGroup(CoreInstallationConstants.DataTypePropertyGroupNames.Content);
            basePageDocumentType.AddPropertyType(GetGridPropertyType(CoreInstallationConstants.DataTypeNames.ContentGrid), CoreInstallationConstants.DataTypePropertyGroupNames.Content);

            contentService.Save(basePageDocumentType);
        }

        private void CreateHomePage()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var homePage = contentService.GetContentType(CoreInstallationConstants.DocumentTypeAliases.HomePage);
            if (homePage != null) return;

            homePage = GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);

            homePage.Name = CoreInstallationConstants.DocumentTypeNames.HomePage;
            homePage.Alias = CoreInstallationConstants.DocumentTypeAliases.HomePage;
            homePage.Icon = CoreInstallationConstants.DocumentTypeIcons.HomePage;
            homePage.AllowedAsRoot = true;

            contentService.Save(homePage);
        }

        private void CreateErrorPage()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var errorPage = contentService.GetContentType(CoreInstallationConstants.DocumentTypeAliases.ErrorPage);
            if (errorPage != null) return;

            errorPage = GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithContentGrid);

            errorPage.Name = CoreInstallationConstants.DocumentTypeNames.ErrorPage;
            errorPage.Alias = CoreInstallationConstants.DocumentTypeAliases.ErrorPage;
            errorPage.Icon = CoreInstallationConstants.DocumentTypeIcons.ErrorPage;

            contentService.Save(errorPage);
            AddAllowedChildNode(CoreInstallationConstants.DocumentTypeAliases.HomePage, CoreInstallationConstants.DocumentTypeAliases.ErrorPage);
        }

        private void CreateContentPage()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var contentPage = contentService.GetContentType(CoreInstallationConstants.DocumentTypeAliases.ContentPage);
            if (contentPage != null) return;

            contentPage = GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithContentGrid);

            contentPage.Name = CoreInstallationConstants.DocumentTypeNames.ContentPage;
            contentPage.Alias = CoreInstallationConstants.DocumentTypeAliases.ContentPage;
            contentPage.Icon = CoreInstallationConstants.DocumentTypeIcons.ContentPage;

            contentService.Save(contentPage);
            AddAllowedChildNode(CoreInstallationConstants.DocumentTypeAliases.HomePage, CoreInstallationConstants.DocumentTypeAliases.ContentPage);
            AddAllowedChildNode(CoreInstallationConstants.DocumentTypeAliases.ContentPage, CoreInstallationConstants.DocumentTypeAliases.ContentPage);
        }

        public static PropertyType GetGridPropertyType(string gridTypeName)
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var defaultGridDataType = dataTypeService.GetDataTypeDefinitionByName(gridTypeName);
            var gridProperty = new PropertyType(defaultGridDataType)
            {
                Name = CoreInstallationConstants.DataTypePropertyNames.Grid,
                Alias = CoreInstallationConstants.DataTypePropertyAliases.Grid
            };

            return gridProperty;
        }

        public static ContentType GetBasePageWithGridBase(string basePageTypeAlias)
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;
            var fileService = ApplicationContext.Current.Services.FileService;

            var basePageWithGrid = contentService.GetContentType(basePageTypeAlias);
            var basePageWithGridBase = new ContentType(basePageWithGrid.Id);

            basePageWithGridBase.AddContentType(basePageWithGrid);
            basePageWithGridBase.SetDefaultTemplate(fileService.GetTemplate(CoreInstallationConstants.DocumentTypeAliases.GridPageLayoutTemplateAlias));

            return basePageWithGridBase;
        }

        private void CreateGridPageLayoutTemplate()
        {
            var fileService = ApplicationContext.Current.Services.FileService;
            var alias = CoreInstallationConstants.DocumentTypeAliases.GridPageLayoutTemplateAlias;
            var gridPageLayoutTemplate = fileService.GetTemplate(alias);
            if (gridPageLayoutTemplate != null) return;

            gridPageLayoutTemplate = new Template(alias, alias);

            var layoutEmbeddedResourceFileName = "uIntra.Core.Installer.PreValues.GridPageLayout.cshtml";
            gridPageLayoutTemplate.Content = GetEmbeddedResourceValue(layoutEmbeddedResourceFileName);

            fileService.SaveTemplate(gridPageLayoutTemplate);
        }

        public static void AddAllowedChildNode(string parentDocumentTypeAlias, string childDocumentTypeAlias)
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;
            var parentNodeDataType = contentService.GetContentType(parentDocumentTypeAlias);
            var childNodeDataType = contentService.GetContentType(childDocumentTypeAlias);
            var allowedChildren = parentNodeDataType.AllowedContentTypes.ToList();
            var isChildAlready = allowedChildren.Any(c => c.Id.Value == childNodeDataType.Id);
            if (isChildAlready)
            {
                return;
            }

            allowedChildren.Add(new ContentTypeSort(childNodeDataType.Id, 1));
            parentNodeDataType.AllowedContentTypes = allowedChildren;

            contentService.Save(parentNodeDataType);
        }

        private static void AddImageCropperPreset()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var imageCropperDataType = dataTypeService.GetDataTypeDefinitionByName(CoreInstallationConstants.DataTypeNames.ImageCropper);
            var preValues = dataTypeService.GetPreValuesCollectionByDataTypeId(imageCropperDataType.Id);
            var preValuesDictionary = preValues.PreValuesAsDictionary;

            var preValuesArray = new JArray();

            if (preValuesDictionary.ContainsKey(CoreInstallationConstants.DataTypePropertyPreValues.ImageCropperPresetsAlias))
            {
                preValuesArray = JArray.Parse(preValues.PreValuesAsDictionary[CoreInstallationConstants.DataTypePropertyPreValues.ImageCropperPresetsAlias].Value);
                foreach (var child in preValuesArray.Children())
                {
                    if (child.Value<string>("alias") == CoreInstallationConstants.DataTypePropertyPreValues.ImageCropperPresetDictionaryName)
                    {
                        return;
                    }
                }
            }

            var preset = new
            {
                alias = CoreInstallationConstants.DataTypePropertyPreValues.ImageCropperPresetDictionaryName,
                height = CoreInstallationConstants.DataTypePropertyPreValues.ImageCropperPresetHeigth,
                width = CoreInstallationConstants.DataTypePropertyPreValues.ImageCropperPresetWidth
            };

            preValuesArray.Add(JObject.FromObject(preset));

            var newVal = preValuesArray.ToString();
            preValues.PreValuesAsDictionary.Add(CoreInstallationConstants.DataTypePropertyPreValues.ImageCropperPresetsAlias, new PreValue(newVal));
            dataTypeService.SavePreValues(imageCropperDataType, preValues.PreValuesAsDictionary);
        }

        private void CreateLinksPickerDataType()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;

            var folderTypeDataType = dataTypeService.GetDataTypeDefinitionByName(CoreInstallationConstants.DataTypeNames.LinksPicker);
            if (folderTypeDataType == null)
            {
                folderTypeDataType = new DataTypeDefinition(-1, CoreInstallationConstants.DataTypePropertyEditors.LinksPicker)
                {
                    Name = CoreInstallationConstants.DataTypeNames.LinksPicker
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

        public static void CreateTrueFalseDataType(string name)
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;

            var dataType = dataTypeService.GetDataTypeDefinitionByName(name);
            if (dataType == null)
            {
                dataType = new DataTypeDefinition("Umbraco.TrueFalse")
                {
                    Name = name
                };

                dataTypeService.Save(dataType);
            }
        }

        public static void InheritCompositionForPage(string pageTypeAlias, string compositionTypeAlias)
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var page = contentService.GetContentType(pageTypeAlias);
            var composition = contentService.GetContentType(compositionTypeAlias);
            if (page == null || composition == null) return;

            if (page.ContentTypeCompositionExists(composition.Alias)) return;

            page.AddContentType(composition);
            contentService.Save(page);
        }

        public static string GetEmbeddedResourceValue(string embeddedResourceName, Assembly sourceAssembly = null)
        {
            var assembly = sourceAssembly != null ? sourceAssembly : Assembly.GetCallingAssembly();
            string json;
            using (Stream stream = assembly.GetManifestResourceStream(embeddedResourceName))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException($"Embedded resource {embeddedResourceName} doesn't exist.");
                }
                using (TextReader reader = new StreamReader(stream))
                {
                    json = reader.ReadToEnd();
                }

            }

            return json;
        }

        public static IContentType CreatePageDocTypeWithBaseGrid(BasePageWithDefaultGridCreateModel model)
        {
            if (!ValidateCreationModel(model))
            {
                return null;
            }
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var page = contentService.GetContentType(model.Alias);
            if (page != null) return null;

            page = GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);

            page.Name = model.Name;
            page.Alias = model.Alias;
            page.Icon = model.Icon;

            contentService.Save(page);
            if (model.ParentAlias.IsNotNullOrEmpty())
            {
                AddAllowedChildNode(model.ParentAlias, model.Alias);
            }

            return page;
        }

        private static bool ValidateCreationModel(BasePageWithDefaultGridCreateModel model)
        {
            var context = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(model, context, validationResults, true);
        }
    }
}
