using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Constants;
using uIntra.Core.Installer;
using uIntra.Core.Installer.Migrations;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using static uIntra.Search.Installer.SearchInstallationConstants.DocumentTypeAliases;

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
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var searchResultPage = contentService.GetContentType(SearchResultPage);
            if (searchResultPage != null) return;

            searchResultPage = CoreInstallationStep_0_0_1.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);
            //TODO: Move static methods to service

            searchResultPage.Name = SearchInstallationConstants.DocumentTypeNames.SearchResultPage;
            searchResultPage.Alias = SearchResultPage;
            searchResultPage.Icon = SearchInstallationConstants.DocumentTypeIcons.SearchResultPage;

            contentService.Save(searchResultPage);
            CoreInstallationStep_0_0_1.AddAllowedChildNode(CoreInstallationConstants.DocumentTypeAliases.HomePage, SearchResultPage);
        }

        private void CreateContentPageUseInSearchTrueFalse()
        {
            CoreInstallationStep_0_0_1.CreateTrueFalseDataType(SearchInstallationConstants.DataTypeNames.ContentPageUseInSearch);
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
