using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Hosting;
using Newtonsoft.Json.Linq;
using uIntra.Core.Extentions;
using uIntra.Core.Media;
using uIntra.Core.Migrations;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace uIntra.Core.Installer
{
    public class CoreInstallationStep : IIntranetInstallationStep
    {
        public string PackageName => "uIntra.Core";
        public int Priority => 0;

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
            var configPath = HostingEnvironment.MapPath("~/Installer/PreValues/DefaultGridPreValues.json");
            CreateGrid(CoreInstallationConstants.DataTypeNames.DefaultGrid, configPath);
        }
        private void CreateContentGridDataType()
        {
            var configPath = HostingEnvironment.MapPath("~/Installer/PreValues/ContentGridPreValues.json");
            CreateGrid(CoreInstallationConstants.DataTypeNames.ContentGrid, configPath);
        }

        private void CreateGrid(string dataTypeName, string configFilePath)
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var defaultGridDataType = dataTypeService.GetDataTypeDefinitionByName(dataTypeName);
            if (defaultGridDataType != null) return;

            var jsonPrevalues = JObject.Parse(System.IO.File.ReadAllText(configFilePath));
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

            var path = HostingEnvironment.MapPath("~/Installer/PreValues/GridPageLayout.cshtml");
            gridPageLayoutTemplate.Content = System.IO.File.ReadAllText(path);

            fileService.SaveTemplate(gridPageLayoutTemplate);
        }

        public static void AddAllowedChildNode(string parentDocumentTypeAlias, string childDocumentTypeAlias)
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;
            var parentNodeDataType = contentService.GetContentType(parentDocumentTypeAlias);
            var childNodeDataType = contentService.GetContentType(childDocumentTypeAlias);
            var allowedChilds = parentNodeDataType.AllowedContentTypes.ToList();

            allowedChilds.Add(new ContentTypeSort(childNodeDataType.Id, 1));
            parentNodeDataType.AllowedContentTypes = allowedChilds;

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

            var dataType = dataTypeService.GetDataTypeDefinitionByName(ImageConstants.IsDeletedDataTypeDefinitionName);
            if (dataType == null)
            {
                dataType = new DataTypeDefinition("Umbraco.TrueFalse")
                {
                    Name = ImageConstants.IsDeletedDataTypeDefinitionName
                };

                dataTypeService.Save(dataType);
            }

            var isDeletedPropertyType = new PropertyType(dataType)
            {
                Name = "Is deleted",
                Alias = ImageConstants.IsDeletedPropertyTypeAlias
            };

            if (!imageType.PropertyTypeExists(isDeletedPropertyType.Alias))
            {
                imageType.AddPropertyType(isDeletedPropertyType);
                contentTypeService.Save(imageType);
            }

            if (!fileType.PropertyTypeExists(isDeletedPropertyType.Alias))
            {
                fileType.AddPropertyType(isDeletedPropertyType);
                contentTypeService.Save(fileType);
            }
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

        private static void AddIntranetUserIdProperty()
        {
            var contentTypeService = ApplicationContext.Current.Services.ContentTypeService;

            var imageType = contentTypeService.GetMediaType(UmbracoAliases.Media.ImageTypeAlias);
            var fileType = contentTypeService.GetMediaType(UmbracoAliases.Media.FileTypeAlias);

            var userIdPropertyType = new PropertyType("Umbraco.NoEdit", DataTypeDatabaseType.Nvarchar, ImageConstants.IntranetCreatorId)
            {
                Name = "Intranet user id"
            };

            if (!imageType.PropertyTypeExists(userIdPropertyType.Alias))
            {
                imageType.AddPropertyType(userIdPropertyType);
                contentTypeService.Save(imageType);
            }

            if (!fileType.PropertyTypeExists(userIdPropertyType.Alias))
            {
                fileType.AddPropertyType(userIdPropertyType);
                contentTypeService.Save(fileType);
            }
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
    }
}
