using uIntra.Core.Installer;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace uIntra.Search.Installer.Migrations
{
    public class SearchInstallationStep_0_0_1 : IIntranetInstallationStep
    {
        public string PackageName => "uIntra.Search";
        public int Priority => 2;
        public string Version => InstallationVersionConstrants.Version_0_0_1;


        public void Execute()
        {
            CreateSearchResultPage();
            CreateContentPageUseInSearchTrueFalse();
            AddSearchToContentPage();
        }


        private void CreateSearchResultPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = SearchInstallationConstants.DocumentTypeNames.SearchResultPage,
                Alias = SearchInstallationConstants.DocumentTypeAliases.SearchResultPage,
                Icon = SearchInstallationConstants.DocumentTypeIcons.SearchResultPage,
                ParentAlias = CoreInstallationConstants.DocumentTypeAliases.HomePage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateContentPageUseInSearchTrueFalse()
        {
            InstallationStepsHelper.CreateTrueFalseDataType(SearchInstallationConstants.DataTypeNames.ContentPageUseInSearch);
        }

        private void AddSearchToContentPage()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var contentPage = contentService.GetContentType(CoreInstallationConstants.DocumentTypeAliases.ContentPage);
            if (contentPage == null) return;

            var useInSearchDataType = dataTypeService.GetDataTypeDefinitionByName(SearchInstallationConstants.DataTypeNames.ContentPageUseInSearch);
            var useInSearchProperty = new PropertyType(useInSearchDataType)
            {
                Name = SearchInstallationConstants.DocumentTypePropertyNames.UseInSearch,
                Alias = SearchInstallationConstants.DocumentTypePropertyAliases.UseInSearch
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
