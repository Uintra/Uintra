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
using static uIntra.Search.Installer.SearchInstallationConstants.MediaAliases;

namespace uIntra.Search.Installer.Migrations
{
    public class SearchInstallationStep_0_0_1 : IIntranetInstallationStep
    {
        public string PackageName => "uIntra.Search";
        public int Priority => 2;
        public string Version => InstallationVersionConstrants.Version_0_0_1;

        private readonly IContentTypeService _contentTypeService = ApplicationContext.Current.Services.ContentTypeService;
        private readonly IMediaService _mediaService = ApplicationContext.Current.Services.MediaService;
        private readonly IDataTypeService _dataTypeService = ApplicationContext.Current.Services.DataTypeService;

        private readonly string[] searchableMediaAliases =
            {UmbracoAliases.Media.FileTypeAlias, UmbracoAliases.Media.ImageTypeAlias};

        public void Execute()
        {
            CreateSearchResultPage();
            CreateContentPageUseInSearchTrueFalse();
            CreateMediaUseInSearch();
            AddSearchToContentPage();
        }

        private void CreateMediaUseInSearch()
        {
            var searchMediaCompositionType = _contentTypeService.GetMediaType(SearchMediaCompositionAlias) ?? CreateSearchMediaComposition();
            var searchableMediaTypes = GetSearchableMediaTypes();

            foreach (var media in searchableMediaTypes)
            {
                media.AddContentType(searchMediaCompositionType);               
                _contentTypeService.Save(media);
            }
        }

        private IEnumerable<IMediaType> GetSearchableMediaTypes()
        {
            return searchableMediaAliases.Select(_contentTypeService.GetMediaType);
        }

        private MediaType CreateSearchMediaComposition()
        {
            var compositionFolder = GetMediaCompositionFolder();
            var composition = new MediaType(compositionFolder.Id)
            {
                Alias = SearchMediaCompositionAlias,
                Name = SearchMediaCompositionName,
                SortOrder = 2
            };

            CoreInstallationStep_0_0_1.CreateTrueFalseDataType("TrueFalse");
            var trueFalseEditor = _dataTypeService.GetDataTypeDefinitionByName("TrueFalse");
            var property = new PropertyType(trueFalseEditor, UseInSearch)
            {
                Name = "Use in search",

            };
            composition.AddPropertyType(property, CompositionTabName);
            _contentTypeService.Save(composition);

            return composition;
        }

        private EntityContainer GetMediaCompositionFolder()
        {
            var folder = _contentTypeService.GetMediaTypeContainers(CompositionFolder, 1).FirstOrDefault();
            return folder ?? CreateMediaCompositionFolder();
        }

        private EntityContainer CreateMediaCompositionFolder()
        {
            _contentTypeService.CreateMediaTypeContainer(-1, CompositionFolder);
            return GetMediaCompositionFolder();
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
