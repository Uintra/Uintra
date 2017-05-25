using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using uIntra.Core.Extentions;
using uIntra.Core.Media;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace uIntra.Core.Migrations
{
    public static class MediaMigrations
    {
        public static void AddIntranetUserIdProperty()
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
        }

        public static void AddFolderProperties()
        {
            var contentTypeService = ApplicationContext.Current.Services.ContentTypeService;
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
                Name = FolderConstants.FolderTypePropertyTypeName,
                Alias = FolderConstants.FolderTypePropertyTypeAlias
            };

            if (!folderType.PropertyTypeExists(folderTypePropertyType.Alias))
            {
                folderType.AddPropertyType(folderTypePropertyType);
            }

            var allowedMediaExtensionsPropertyType = new PropertyType("Umbraco.Textbox", DataTypeDatabaseType.Nvarchar)
            {
                Alias = FolderConstants.AllowedMediaExtensionsPropertyTypeAlias,
                Name = FolderConstants.AllowedMediaExtensionsPropertyTypeName
            };

            if (!folderType.PropertyTypeExists(allowedMediaExtensionsPropertyType.Alias))
            {
                folderType.AddPropertyType(allowedMediaExtensionsPropertyType);
            }

            contentTypeService.Save(folderType);

        }

        public static void CreateDefaultFolders()
        {
            var mediaService = ApplicationContext.Current.Services.MediaService;
            var folderTypes = Enum.GetValues(typeof(MediaFolderTypeEnum)).Cast<MediaFolderTypeEnum>();
            var rootFolders = mediaService.GetRootMedia().Where(f => f.ContentType.Alias.Equals(UmbracoAliases.Media.FolderTypeAlias));
            foreach (var folderType in folderTypes)
            {
                var folderName = folderType.GetAttribute<DisplayAttribute>().Name;
                var folderByName = rootFolders.Where(m => m.Name.Equals(folderName));
                var folderByType = rootFolders.Where(m => m.GetValue<MediaFolderTypeEnum>(FolderConstants.FolderTypePropertyTypeAlias) == folderType);

                if (folderByName.Any() || folderByType.Any())
                {
                    continue;
                }

                var mediaFolder = mediaService.CreateMedia(folderName, -1, UmbracoAliases.Media.FolderTypeAlias);
                mediaFolder.SetValue(FolderConstants.FolderTypePropertyTypeAlias, folderType.ToString());
                mediaService.Save(mediaFolder);
            }
        }

        public static void Migrate()
        {
            AddIntranetUserIdProperty();
            AddFolderProperties();
            CreateDefaultFolders();
        }
    }
}