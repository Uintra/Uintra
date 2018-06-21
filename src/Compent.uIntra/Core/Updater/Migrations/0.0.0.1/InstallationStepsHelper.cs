using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Compent.Extensions;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Steps.AggregateSubsteps;
using Localization.Core;
using Newtonsoft.Json.Linq;
using Uintra.Core.Constants;
using Uintra.Core.Utils;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace Compent.Uintra.Core.Updater.Migrations._0._0._0._1
{
    public class InstallationStepsHelper
    {
        public static ContentType GetBasePageWithGridBase(string basePageTypeAlias)
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;
            var fileService = ApplicationContext.Current.Services.FileService;

            var basePageWithGrid = contentService.GetContentType(basePageTypeAlias);
            var basePageWithGridBase = new ContentType(basePageWithGrid.Id);

            basePageWithGridBase.AddContentType(basePageWithGrid);
            basePageWithGridBase.SetDefaultTemplate(fileService.GetTemplate(CoreInstallationConstants.TemplateAliases.GridPageLayoutTemplateAlias));

            return basePageWithGridBase;
        }

        public static IContentType CreatePageDocTypeWithBaseGrid(BasePageWithDefaultGridCreateModel model)
        {
            return CreatePageDocTypeWithGrid(model, CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);
        }

        public static IContentType CreatePageDocTypeWithContentGrid(BasePageWithDefaultGridCreateModel model)
        {
            return CreatePageDocTypeWithGrid(model, CoreInstallationConstants.DocumentTypeAliases.BasePageWithContentGrid);
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

        private static bool ValidateCreationModel(BasePageWithDefaultGridCreateModel model)
        {
            var context = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(model, context, validationResults, true);
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

        public static void DeleteCompositionFromPage(string pageTypeAlias, string compositionTypeAlias)
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var page = contentService.GetContentType(pageTypeAlias);
            var composition = contentService.GetContentType(compositionTypeAlias);
            if (page == null || composition == null) return;

            if (!page.ContentTypeCompositionExists(composition.Alias)) return;

            page.RemoveContentType(composition.Alias);
            contentService.Save(page);
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

        public static  void AddIntranetUserIdProperty(IMediaType mediaType)
        {
            var contentTypeService = ApplicationContext.Current.Services.ContentTypeService;

            var mediatypeIntranetUserId = GetIntranetUserIdPropertyType();
            if (!mediaType.PropertyTypeExists(mediatypeIntranetUserId.Alias))
            {
                mediaType.AddPropertyType(mediatypeIntranetUserId);
                contentTypeService.Save(mediaType);
            }
        }

        private static PropertyType GetIntranetUserIdPropertyType()
        {
            return new PropertyType("Umbraco.NoEdit", DataTypeDatabaseType.Nvarchar)
            {
                Name = "Intranet user id",
                Alias = IntranetConstants.IntranetCreatorId
            };
        }

        public static void AddIsDeletedProperty(IMediaType mediaType)
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var contentTypeService = ApplicationContext.Current.Services.ContentTypeService;

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
            if (!mediaType.PropertyTypeExists(imageIsDeletedPropertyType.Alias))
            {
                mediaType.AddPropertyType(imageIsDeletedPropertyType);
                contentTypeService.Save(mediaType);
            }
        }

        private static PropertyType GetIsDeletedPropertyType(IDataTypeDefinition dataType)
        {
            return new PropertyType(dataType)
            {
                Name = "Is deleted",
                Alias = UmbracoAliases.Media.IsDeletedPropertyTypeAlias
            };
        }

        public static void CreateGrid(string dataTypeName, string gridEmbeddedResourceFileName)
        {
            var sourceAssembly = Assembly.GetCallingAssembly();
            CreateGrid(dataTypeName, gridEmbeddedResourceFileName, sourceAssembly);
        }

        public static void CreateGrid(string dataTypeName, string gridEmbeddedResourceFileName, Assembly sourceAssembly)
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var defaultGridDataType = dataTypeService.GetDataTypeDefinitionByName(dataTypeName);
            if (defaultGridDataType != null) return;

            var gridJson = EmbeddedResourcesUtils.ReadResourceContent(gridEmbeddedResourceFileName, sourceAssembly);

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

        public static void AddTranslation(string key, string translation)
        {
            var localizationCoreService = DependencyResolver.Current.GetService<ILocalizationCoreService>();

            var resourceModel = localizationCoreService.GetResourceModel(key);
            if (resourceModel.Translations[Constants.LocalizationConstants.CultureKeys.English].Contains(key))
            {
                resourceModel.Translations[Constants.LocalizationConstants.CultureKeys.English] = translation;
                localizationCoreService.Create(resourceModel);
            }
        }

        public static void UpdateTranslation(string key, string oldTranslation, string newTranslation)
        {
            var localizationCoreService = DependencyResolver.Current.GetService<ILocalizationCoreService>();

            var resourceModel = localizationCoreService.GetResourceModel(key);
            if (resourceModel.Translations[Constants.LocalizationConstants.CultureKeys.English].Contains(oldTranslation))
            {
                resourceModel.Translations[Constants.LocalizationConstants.CultureKeys.English] = newTranslation;
                localizationCoreService.Update(resourceModel);
            }
        }

        public static void SetGridPageLayoutTemplateContent(string layoutEmbeddedResourceFileName)
        {
            var fileService = ApplicationContext.Current.Services.FileService;
            var alias = CoreInstallationConstants.TemplateAliases.GridPageLayoutTemplateAlias;

            var gridPageLayoutTemplate = fileService.GetTemplate(alias) ?? new Template(alias, alias);
            gridPageLayoutTemplate.Content = EmbeddedResourcesUtils.ReadResourceContent(layoutEmbeddedResourceFileName);

            fileService.SaveTemplate(gridPageLayoutTemplate);
        }

        private static IContentType CreatePageDocTypeWithGrid(BasePageWithDefaultGridCreateModel model, string basePageTypeAlias)
        {
            if (!ValidateCreationModel(model)) return null;

            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var page = contentService.GetContentType(model.Alias);
            if (page != null) return page;

            page = GetBasePageWithGridBase(basePageTypeAlias);

            page.Name = model.Name;
            page.Alias = model.Alias;
            page.Icon = model.Icon;

            contentService.Save(page);
            if (model.ParentAlias.HasValue())
            {
                AddAllowedChildNode(model.ParentAlias, model.Alias);
            }

            return page;
        }
    }
}
