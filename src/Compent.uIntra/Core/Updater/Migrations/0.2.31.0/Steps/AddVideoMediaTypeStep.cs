using Compent.uIntra.Core.Updater.Migrations._0._0._0._1.Constants;
using Compent.uIntra.Core.Updater.Migrations._0._2._31._0.Constants;
using uIntra.Core.Installer;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Compent.uIntra.Core.Updater.Migrations._0._2._31._0.Steps
{
    public class AddVideoMediaTypeStep : IMigrationStep
    {
        private readonly IContentTypeService _contentTypeService;

        public AddVideoMediaTypeStep(IContentTypeService contentTypeService)
        {
            _contentTypeService = contentTypeService;
        }

        public ExecutionResult Execute()
        {
            CreateMediaTypeWithProperties();
            return ExecutionResult.Success;
        }

        public void Undo()
        {
            var videoMediaType = _contentTypeService.GetMediaType(AddVideoMediaTypeStepConstants.DocumentTypeAliases.Video);
            if (videoMediaType != null)
            {
                _contentTypeService.Delete(videoMediaType);
            }
        }

        public void CreateMediaTypeWithProperties()
        {
            var videoMediaType = _contentTypeService.GetMediaType(AddVideoMediaTypeStepConstants.DocumentTypeAliases.Video);
            if (videoMediaType != null) return;

            videoMediaType = CreateVideoMediaType();

            var searchMediaCompositionType = _contentTypeService.GetMediaType(SearchInstallationConstants.MediaAliases.SearchMediaCompositionAlias);
            videoMediaType.AddContentType(searchMediaCompositionType);

            videoMediaType.AddPropertyGroup(AddVideoMediaTypeStepConstants.DocumentTypeTabNames.File);

            AddUploadFileProperty(videoMediaType);
            AddLabelProperty(videoMediaType, AddVideoMediaTypeStepConstants.DocumentTypePropertyNames.Width, AddVideoMediaTypeStepConstants.DocumentTypePropertyAliases.VideoWidth, DataTypeDatabaseType.Integer);
            AddLabelProperty(videoMediaType, AddVideoMediaTypeStepConstants.DocumentTypePropertyNames.Height, AddVideoMediaTypeStepConstants.DocumentTypePropertyAliases.VideoHeight, DataTypeDatabaseType.Integer);
            AddLabelProperty(videoMediaType, AddVideoMediaTypeStepConstants.DocumentTypePropertyNames.Size, AddVideoMediaTypeStepConstants.DocumentTypePropertyAliases.UmbracoBytes, DataTypeDatabaseType.Integer);
            AddLabelProperty(videoMediaType, AddVideoMediaTypeStepConstants.DocumentTypePropertyNames.Type, AddVideoMediaTypeStepConstants.DocumentTypePropertyAliases.UmbracoExtension, DataTypeDatabaseType.Nvarchar);
            AddLabelProperty(videoMediaType, AddVideoMediaTypeStepConstants.DocumentTypePropertyNames.ThumbnailUrl, AddVideoMediaTypeStepConstants.DocumentTypePropertyAliases.ThumbnailUrl, DataTypeDatabaseType.Nvarchar);

            _contentTypeService.Save(videoMediaType);

            InstallationStepsHelper.AddIsDeletedProperty(videoMediaType);
            InstallationStepsHelper.AddIntranetUserIdProperty(videoMediaType);
        }

        private static MediaType CreateVideoMediaType()
        {
            return new MediaType(-1)
            {
                Alias = AddVideoMediaTypeStepConstants.DocumentTypeAliases.Video,
                Name = AddVideoMediaTypeStepConstants.DocumentTypeNames.Video,
                Icon = AddVideoMediaTypeStepConstants.DocumentTypeIcons.Video
            };
        }

        private static void AddUploadFileProperty(IContentTypeBase mediaType)
        {
            var uploadFilePropertyType = new PropertyType("Umbraco.UploadField", DataTypeDatabaseType.Nvarchar)
            {
                Name = AddVideoMediaTypeStepConstants.DocumentTypePropertyNames.UploadFile,
                Alias = AddVideoMediaTypeStepConstants.DocumentTypePropertyAliases.UmbracoFile
            };

            if (mediaType.PropertyTypeExists(uploadFilePropertyType.Alias)) return;

            mediaType.AddPropertyType(uploadFilePropertyType, AddVideoMediaTypeStepConstants.DocumentTypeTabNames.File);
        }

        private static void AddLabelProperty(IContentTypeBase mediaType, string name, string alias, DataTypeDatabaseType dataTypeDatabaseType)
        {
            var intranetUserIdPropertyType = GetLabelPropertyType(name, alias, dataTypeDatabaseType);

            if (mediaType.PropertyTypeExists(intranetUserIdPropertyType.Alias)) return;

            mediaType.AddPropertyType(intranetUserIdPropertyType, AddVideoMediaTypeStepConstants.DocumentTypeTabNames.File);
        }

        private static PropertyType GetLabelPropertyType(string name, string alias, DataTypeDatabaseType dataTypeDatabaseType)
        {
            return new PropertyType("Umbraco.NoEdit", dataTypeDatabaseType) { Name = name, Alias = alias };
        }
    }
}