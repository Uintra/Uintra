using Umbraco.Core;
using Umbraco.Core.Models;

namespace uIntra.Groups.Installer.Migrations
{
    public static class GroupMigrations
    {
        public static void Migrate()
        {
            var contentTypeService = ApplicationContext.Current.Services.ContentTypeService;

            var folderType = contentTypeService.GetMediaType("Folder");

            var groupIdProperty = new PropertyType("Umbraco.NoEdit", DataTypeDatabaseType.Nvarchar, "GroupId")
            {
                Name = "Group Id"
            };

            if (!folderType.PropertyTypeExists(groupIdProperty.Alias))
            {
                folderType.AddPropertyType(groupIdProperty);
                contentTypeService.Save(folderType);
            }
        }
    }
}