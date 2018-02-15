using System.Linq;
using Compent.Uintra.Core.Constants;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using Compent.Uintra.Core.Updater.Migrations._0._2._31._0.Constants;
using Uintra.Core.Installer;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Compent.Uintra.Core.Updater.Migrations._0._2._31._0.Steps
{
    public class UserTagsInstallationStep : IMigrationStep
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IContentService _contentService;

        public UserTagsInstallationStep(UmbracoHelper umbracoHelper, IContentService contentService)
        {
            _umbracoHelper = umbracoHelper;
            _contentService = contentService;
        }

        ExecutionResult IMigrationStep.Execute()
        {
            CreateUserTagDocumentType();
            CreateUserTagsFolderDocumentType();

            CreateUserTagsFolder();

            return ExecutionResult.Success;
        }

        public void Undo()
        {
            throw new System.NotImplementedException();
        }
        private void CreateUserTagDocumentType()
        {
            var contentTypeService = ApplicationContext.Current.Services.ContentTypeService;
            var userTagDocumentType = contentTypeService.GetContentType(TaggingInstallationConstants.DocumentTypeAliases.UserTag);
            if (userTagDocumentType != null) return;

            var dataContentFolder = contentTypeService.GetContentTypeContainers(
                folderName: CoreInstallationConstants.DocumentTypesContainerNames.DataContent, 
                level:1)
                .First();

            userTagDocumentType = new ContentType(dataContentFolder.Id)
            {
                Name = TaggingInstallationConstants.DocumentTypeNames.UserTag,
                Alias = TaggingInstallationConstants.DocumentTypeAliases.UserTag,
                Icon = TaggingInstallationConstants.DocumentTypeIcons.UserTag
            };

            userTagDocumentType.AddPropertyGroup(TaggingInstallationConstants.DocumentTypeTabNames.Content);

            var textProperty = new PropertyType("Umbraco.Textbox", DataTypeDatabaseType.Nvarchar)
            {
                Alias = "text",
                Name = "Text",
                Mandatory = true
            };

            userTagDocumentType.AddPropertyType(textProperty, TaggingInstallationConstants.DocumentTypeTabNames.Content);

            contentTypeService.Save(userTagDocumentType);
        }

        private void CreateUserTagsFolderDocumentType()
        {
            var contentTypeService = ApplicationContext.Current.Services.ContentTypeService;

            var dataFolderDocType = contentTypeService.GetContentType(TaggingInstallationConstants.DocumentTypeAliases.UserTagFolder);
            if (dataFolderDocType != null) return;

            var folder = contentTypeService.GetContentTypeContainers(CoreInstallationConstants.DocumentTypesContainerNames.Folders, 1).First();
            dataFolderDocType = new ContentType(folder.Id)
            {
                Name = TaggingInstallationConstants.DocumentTypeNames.UserTagFolder,
                Alias = TaggingInstallationConstants.DocumentTypeAliases.UserTagFolder
            };

            contentTypeService.Save(dataFolderDocType);

            InstallationStepsHelper.AddAllowedChildNode(TaggingInstallationConstants.DocumentTypeAliases.UserTagFolder, TaggingInstallationConstants.DocumentTypeAliases.UserTag);
            InstallationStepsHelper.AddAllowedChildNode("dataFolder", TaggingInstallationConstants.DocumentTypeAliases.UserTagFolder);
        }


        private void CreateUserTagsFolder()
        {
            var dataFolder = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.DataFolder));
            if (dataFolder.Children.Any(el => el.DocumentTypeAlias.Equals(TaggingInstallationConstants.DocumentTypeAliases.UserTagFolder)))
            {
                return;
            }

            var content = _contentService.CreateContentWithIdentity(TaggingInstallationConstants.ContentDefaultName.UserTagFolder, dataFolder.Id, TaggingInstallationConstants.DocumentTypeAliases.UserTagFolder);

            _contentService.SaveAndPublishWithStatus(content);
        }

    }
}
