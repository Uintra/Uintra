using System.Linq;
using System.Web;
using Compent.uIntra.Core.Constants;
using uIntra.Core.Extensions;
using uIntra.Core.Installer;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using static Compent.uIntra.Installer.Migrations.TaggingInstallationConstants;

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
            var userTagDocumentType = contentTypeService.GetContentType(DocumentTypeAliases.UserTag);
            if (userTagDocumentType != null) return;

            var dataContentFolder = contentTypeService.GetContentTypeContainers(CoreInstallationConstants.DocumentTypesContainerNames.DataContent, 1).First();

            userTagDocumentType = new ContentType(dataContentFolder.Id)
            {
                Name = DocumentTypeNames.UserTag,
                Alias = DocumentTypeAliases.UserTag,
                Icon = DocumentTypeIcons.UserTag
            };

            userTagDocumentType.AddPropertyGroup(DocumentTypeTabNames.Content);

            var textProperty = new PropertyType("Umbraco.Textbox", DataTypeDatabaseType.Nvarchar)
            {
                Alias = "text",
                Name = "Text",
                Mandatory = true
            };

            userTagDocumentType.AddPropertyType(textProperty, DocumentTypeTabNames.Content);

            contentTypeService.Save(userTagDocumentType);
        }

        private void CreateUserTagsFolderDocumentType()
        {
            var contentTypeService = ApplicationContext.Current.Services.ContentTypeService;

            var dataFolderDocType = contentTypeService.GetContentType(DocumentTypeAliases.UserTagFolder);
            if (dataFolderDocType != null) return;

            var folder = contentTypeService.GetContentTypeContainers(CoreInstallationConstants.DocumentTypesContainerNames.Folders, 1).First();
            dataFolderDocType = new ContentType(folder.Id)
            {
                Name = DocumentTypeNames.UserTagFolder,
                Alias = DocumentTypeAliases.UserTagFolder
            };

            contentTypeService.Save(dataFolderDocType);

            InstallationStepsHelper.AddAllowedChildNode(DocumentTypeAliases.UserTagFolder, DocumentTypeAliases.UserTag);
            InstallationStepsHelper.AddAllowedChildNode("dataFolder", DocumentTypeAliases.UserTagFolder);
        }


        private void CreateUserTagsFolder()
        {
            var dataFolder = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.DataFolder));
            if (dataFolder.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliases.UserTagFolder)))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity(DocumentTypeNames.UserTagFolder, dataFolder.Id, DocumentTypeAliases.UserTagFolder);

            _contentService.SaveAndPublishWithStatus(content);
        }
    }
}