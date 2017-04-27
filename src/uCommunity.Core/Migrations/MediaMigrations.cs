using Umbraco.Core;
using Umbraco.Core.Models;

namespace uCommunity.Core.Migrations
{
    public static class MediaMigrations
    {
        public static void Migrate()
        {
            var contentTypeService = ApplicationContext.Current.Services.ContentTypeService;

            var imageType = contentTypeService.GetMediaType("Image");
            var fileType = contentTypeService.GetMediaType("File");

            var userIdPropertyType = new PropertyType("Umbraco.NoEdit", DataTypeDatabaseType.Nvarchar, ImageConstants.IntranetCreatorId)
            {
                Name = "Intranet user id"
            };

            if (!imageType.PropertyTypeExists(userIdPropertyType.Alias))
            {
                imageType.AddPropertyType(userIdPropertyType);
                contentTypeService.Save(imageType);
            }

            if (!fileType.PropertyTypeExists(userIdPropertyType.Alias))
            {
                fileType.AddPropertyType(userIdPropertyType);
                contentTypeService.Save(fileType);
            }
        }
    }
}