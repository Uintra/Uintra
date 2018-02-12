using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Constants;
using Uintra.Core.Installer;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using static Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants.SearchInstallationConstants;
using static Compent.Uintra.Core.Updater.ExecutionResult;

namespace Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Steps
{
    public class MediaSearchInstallationStep : IMigrationStep
    {

        private readonly IContentTypeService _contentTypeService = ApplicationContext.Current.Services.ContentTypeService;
        private readonly IDataTypeService _dataTypeService = ApplicationContext.Current.Services.DataTypeService;
        private readonly IMediaService _mediaService = ApplicationContext.Current.Services.MediaService;

        private readonly string[] searchableMediaAliases =
            {UmbracoAliases.Media.FileTypeAlias, UmbracoAliases.Media.ImageTypeAlias};

        public ExecutionResult Execute()
        {
            CreateMediaUseInSearch();
            return Success;
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        private void CreateMediaUseInSearch()
        {
            var searchMediaCompositionType = _contentTypeService.GetMediaType(MediaAliases.SearchMediaCompositionAlias) ?? CreateSearchMediaComposition();
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
            list.ForEach(m => m.SetValue(MediaAliases.UseInSearch, true));
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
                Alias = MediaAliases.SearchMediaCompositionAlias,
                Name = MediaAliases.SearchMediaCompositionName,
                SortOrder = 2
            };

            InstallationStepsHelper.CreateTrueFalseDataType("TrueFalse");
            var trueFalseEditor = _dataTypeService.GetDataTypeDefinitionByName("TrueFalse");
            var property = new PropertyType(trueFalseEditor, MediaAliases.UseInSearch)
            {
                Name = "Use in search",

            };
            composition.AddPropertyType(property, MediaAliases.CompositionTabName);
            _contentTypeService.Save(composition);

            return composition;
        }

        private EntityContainer GetMediaCompositionFolder()
        {
            var folder = _contentTypeService.GetMediaTypeContainers(MediaAliases.CompositionFolder, 1).FirstOrDefault();
            return folder ?? CreateMediaCompositionFolder();
        }

        private EntityContainer CreateMediaCompositionFolder()
        {
            _contentTypeService.CreateMediaTypeContainer(-1, MediaAliases.CompositionFolder);
            return GetMediaCompositionFolder();
        }
    }
}
