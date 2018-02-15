using System.Linq;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Steps.AggregateSubsteps
{
    public class PagePromotionMigration
    {
        public void Execute()
        {
            CreatePagePromotionDataType();
            CreatePagePromotionComposition();
        }

        private void CreatePagePromotionDataType()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;

            var dataType = dataTypeService.GetDataTypeDefinitionByName(PagePromotionInstallationConstants.DataTypeNames.PagePromotion);
            if (dataType != null) return;

            dataType = new DataTypeDefinition(-1, "custom.PagePromotion")
            {
                Name = PagePromotionInstallationConstants.DataTypeNames.PagePromotion
            };

            dataTypeService.Save(dataType);
        }

        private void CreatePagePromotionComposition()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;

            var compositionFolder = contentService.GetContentTypeContainers(CoreInstallationConstants.DocumentTypesContainerNames.Compositions, 1).Single();

            var pagePromotionCompositionType = contentService.GetContentType(PagePromotionInstallationConstants.DocumentTypeAliases.PagePromotionComposition);
            if (pagePromotionCompositionType != null) return;

            pagePromotionCompositionType = new ContentType(compositionFolder.Id)
            {
                Name = PagePromotionInstallationConstants.DocumentTypeNames.PagePromotionComposition,
                Alias = PagePromotionInstallationConstants.DocumentTypeAliases.PagePromotionComposition
            };

            pagePromotionCompositionType.AddPropertyGroup(PagePromotionInstallationConstants.DocumentTypeTabNames.Promotion);

            var promotionPropertyGroup = pagePromotionCompositionType.PropertyGroups.Single(group => group.Name == PagePromotionInstallationConstants.DocumentTypeTabNames.Promotion);
            promotionPropertyGroup.SortOrder = PagePromotionInstallationConstants.DocumentTypeTabSortOrder.Promotion;

            var pagePromotionDataType = dataTypeService.GetDataTypeDefinitionByName(PagePromotionInstallationConstants.DataTypeNames.PagePromotion);
            var pagePromotionConfigProperty = new PropertyType(pagePromotionDataType)
            {
                Name = PagePromotionInstallationConstants.DocumentTypePropertyNames.PagePromotionConfig,
                Alias = PagePromotionInstallationConstants.DocumentTypePropertyAliases.PagePromotionConfig
            };

            pagePromotionCompositionType.AddPropertyType(pagePromotionConfigProperty, PagePromotionInstallationConstants.DocumentTypeTabNames.Promotion);
            contentService.Save(pagePromotionCompositionType);
        }
    }
}