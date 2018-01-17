using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Constants;
using uIntra.Core.Installer;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using static uIntra.Search.Installer.SearchInstallationConstants.MediaAliases;

namespace uIntra.Search.Installer.Migrations
{
    public class MediaSearchInstallationStep_0_2_1_1 : IIntranetInstallationStep
    {
        public string PackageName { get; } = "uIntra.Search";
        public int Priority => 3;
        public string Version { get; } = "0.2.1.1";

        private readonly IContentTypeService _contentTypeService = ApplicationContext.Current.Services.ContentTypeService;
        private readonly IDataTypeService _dataTypeService = ApplicationContext.Current.Services.DataTypeService;
        private readonly IMediaService _mediaService = ApplicationContext.Current.Services.MediaService;

        private readonly string[] searchableMediaAliases =
            {UmbracoAliases.Media.FileTypeAlias, UmbracoAliases.Media.ImageTypeAlias};

        public void Execute()
        {
            CreateMediaUseInSearch();
        }

        private void CreateMediaUseInSearch()
        {
            var searchMediaCompositionType = _contentTypeService.GetMediaType(SearchMediaCompositionAlias) ?? CreateSearchMediaComposition();
            var searchableMediaTypes = GetSearchableMediaTypes();

            foreach (var type in searchableMediaTypes)
            {
                type.AddContentType(searchMediaCompositionType);
                _contentTypeService.Save(type);
                MakeMediaSearchable(_mediaService.GetMediaOfMediaType(type.Id));
            }
        }

        private void MakeMediaSearchable(IEnumerable<IMedia> medias)
        {
            var list = medias.ToList();
            list.ForEach(m => m.SetValue(UseInSearch, true));
            _mediaService.Save(list);
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

            InstallationStepsHelper.CreateTrueFalseDataType("TrueFalse");
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
    }
}
