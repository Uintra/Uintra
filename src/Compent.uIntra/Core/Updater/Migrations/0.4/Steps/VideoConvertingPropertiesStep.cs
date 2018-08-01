using Uintra.Core.Constants;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace Compent.Uintra.Core.Updater.Migrations._0._4.Steps
{
    public class VideoConvertingPropertiesStep:IMigrationStep
    {
        public ExecutionResult Execute()
        {
            AddConvertInProgressProperty();
            return ExecutionResult.Success;
        }

        public void Undo()
        {

        }

        public void AddConvertInProgressProperty()
        {
            var contentTypeService = ApplicationContext.Current.Services.ContentTypeService;
            
            var videoType = contentTypeService.GetMediaType(UmbracoAliases.Media.VideoTypeAlias);

            var convertInProcessProperty = GetConvertInProcessPropertyType();

            if (!videoType.PropertyTypeExists(convertInProcessProperty.Alias))
            {
                videoType.AddPropertyType(convertInProcessProperty);
                contentTypeService.Save(videoType);
            }

            var thumbnailUrlProperty = GetThumbnailUrlPropertyType();

            if (!videoType.PropertyTypeExists(thumbnailUrlProperty.Alias))
            {
                videoType.AddPropertyType(thumbnailUrlProperty);
                contentTypeService.Save(videoType);
            }
        }

        private PropertyType GetThumbnailUrlPropertyType()
        {
            return new PropertyType("Umbraco.NoEdit", DataTypeDatabaseType.Nvarchar)
            {
                Name = "Thumbnail Url",
                Alias = "thumbnailUrl",                
            };
        }

        private PropertyType GetConvertInProcessPropertyType()
        {
            return new PropertyType("Umbraco.NoEdit", DataTypeDatabaseType.Integer)
            {
                Name = "Convert In Process",
                Alias = "convertInProcess"
            };
        }
    }
}