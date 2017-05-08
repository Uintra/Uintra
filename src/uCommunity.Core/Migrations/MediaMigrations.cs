using System.Collections.Generic;
using uCommunity.Core.Media;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace uCommunity.Core.Migrations
{
    public static class MediaMigrations
    {
        public static void Migrate()
        {
            var contentTypeService = ApplicationContext.Current.Services.ContentTypeService;

            var imageType = contentTypeService.GetMediaType(UmbracoAliases.Media.ImageTypeAlias);
            var fileType = contentTypeService.GetMediaType(UmbracoAliases.Media.FileTypeAlias);

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
            

            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;

            var folderTypeDataType = dataTypeService.GetDataTypeDefinitionByName(FolderConstants.DataTypeName);
            if (folderTypeDataType == null)
            {
                folderTypeDataType = new DataTypeDefinition(-1, UmbracoAliases.EnumDropdownList)
                {
                    Name = FolderConstants.DataTypeName
                };

                var preValues = new Dictionary<string, PreValue>
                {
                    { FolderConstants.PreValueAssemblyAlias, new PreValue(FolderConstants.EnumAssemblyDll)},
                    { FolderConstants.PreValueEnumAlias, new PreValue(typeof(MediaFolderTypeEnum).FullName)}
                };
                dataTypeService.SaveDataTypeAndPreValues(folderTypeDataType, preValues);
            }

            var folderType = contentTypeService.GetMediaType(UmbracoAliases.Media.FolderTypeAlias);

            var folderTypePropertyType = new PropertyType(folderTypeDataType)
            {
                Name = FolderConstants.PropertyTypeName,
                Alias = FolderConstants.PropertyTypeAlias
            };

            if (!folderType.PropertyTypeExists(folderTypePropertyType.Alias))
            {
                folderType.AddPropertyType(folderTypePropertyType);
                contentTypeService.Save(folderType);
            }
        }
    }
}