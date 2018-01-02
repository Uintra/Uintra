using System.Linq;
using System.Web;
using Compent.uIntra.Core.Constants;
using uIntra.Core.Extensions;
using uIntra.Core.Installer;
using uIntra.Users.Installers;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Compent.uIntra.Installer.Migrations
{
    public class TaggingMigration
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IContentService _contentService;

        public TaggingMigration()
        {
            _umbracoHelper = HttpContext.Current.GetService<UmbracoHelper>(); ;
            _contentService = ApplicationContext.Current.Services.ContentService;
        }

        public void Execute()
        {
            CreateUserTagDocumentType();
            CreateUserTagsFolderDocumentType();

            CreateUserTagsFolder();
        }

        private void CreateUserTagDocumentType()
        {
            var contentTypeService = ApplicationContext.Current.Services.ContentTypeService;
            var userTagDocumentType = contentTypeService.GetContentType("userTag");
            if (userTagDocumentType != null) return;

            var dataContentFolder = contentTypeService.GetContentTypeContainers(CoreInstallationConstants.DocumentTypesContainerNames.DataContent, 1).First();

            userTagDocumentType = new ContentType(dataContentFolder.Id)
            {
                Name = "User Tag",
                Alias = "userTag"
            };

            userTagDocumentType.AddPropertyGroup("Content");

            var textProperty = new PropertyType("Umbraco.Textbox", DataTypeDatabaseType.Nvarchar)
            {
                Alias = "text",
                Name = "Text",
                Mandatory = true
            };

            userTagDocumentType.AddPropertyType(textProperty, "Content");

            contentTypeService.Save(userTagDocumentType);
        }

        private void CreateUserTagsFolderDocumentType()
        {
            var contentTypeService = ApplicationContext.Current.Services.ContentTypeService;

            var dataFolderDocType = contentTypeService.GetContentType("userTagFolder");
            if (dataFolderDocType != null) return;

            var folder = contentTypeService.GetContentTypeContainers(CoreInstallationConstants.DocumentTypesContainerNames.Folders, 1).First();
            dataFolderDocType = new ContentType(folder.Id)
            {
                Name = "User Tags Folder",
                Alias = "userTagFolder"
            };

            contentTypeService.Save(dataFolderDocType);

            InstallationStepsHelper.AddAllowedChildNode("userTagFolder", "userTag");
            InstallationStepsHelper.AddAllowedChildNode("dataFolder", "userTagFolder");
        }


        private void CreateUserTagsFolder()
        {
            var dataFolder = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.DataFolder));
            if (dataFolder.Children.Any(el => el.DocumentTypeAlias.Equals("userTagFolder")))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity("User Tags Folder", dataFolder.Id, "userTagFolder");

            _contentService.SaveAndPublishWithStatus(content);
        }
    }
}