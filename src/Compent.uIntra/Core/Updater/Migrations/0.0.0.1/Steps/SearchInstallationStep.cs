using uIntra.Core.Installer;
using Umbraco.Core;
using Umbraco.Core.Models;
using Compent.uIntra.Core.Updater.Migrations._0._0._0._1.Constants;
using Compent.uIntra.Core.Updater.Migrations._0._0._0._1.Steps.AggregateSubsteps;
using static Compent.uIntra.Core.Updater.Migrations._0._0._0._1.Constants.SearchInstallationConstants;
using static Compent.uIntra.Core.Updater.ExecutionResult;

namespace Compent.uIntra.Core.Updater.Migrations._0._0._0._1.Steps
{
    public class SearchInstallationStep : IMigrationStep
    {

        ExecutionResult IMigrationStep.Execute()
        {
            CreateSearchResultPage();
            CreateContentPageUseInSearchTrueFalse();
            AddSearchToContentPage();
            return Success;
        }

        public void Undo()
        {
            throw new System.NotImplementedException();
        }


        private void CreateSearchResultPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = DocumentTypeNames.SearchResultPage,
                Alias = DocumentTypeAliases.SearchResultPage,
                Icon = DocumentTypeIcons.SearchResultPage,
                ParentAlias = CoreInstallationConstants.DocumentTypeAliases.HomePage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateContentPageUseInSearchTrueFalse()
        {
            InstallationStepsHelper.CreateTrueFalseDataType(DataTypeNames.ContentPageUseInSearch);
        }

        private void AddSearchToContentPage()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var contentPage = contentService.GetContentType(CoreInstallationConstants.DocumentTypeAliases.ContentPage);
            if (contentPage == null) return;

            var useInSearchDataType = dataTypeService.GetDataTypeDefinitionByName(DataTypeNames.ContentPageUseInSearch);
            var useInSearchProperty = new PropertyType(useInSearchDataType)
            {
                Name = DocumentTypePropertyNames.UseInSearch,
                Alias = DocumentTypePropertyAliases.UseInSearch
            };
            if (!contentPage.PropertyGroups.Contains("Search"))
            {
                contentPage.AddPropertyGroup("Search");
            }
            if (!contentPage.PropertyTypeExists(useInSearchProperty.Alias))
            {
                contentPage.AddPropertyType(useInSearchProperty, "Search");
            }
            contentService.Save(contentPage);
        }

    }
}
