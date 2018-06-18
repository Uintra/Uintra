using System.Collections.Generic;
using System.Linq;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using Compent.Uintra.Core.Updater.Migrations._0._2._31._0.Constants;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Compent.Uintra.Core.Updater.Migrations._0._2._31._0.Steps
{
    public class UpdateDataFolderNamesMigrationInstallationStep : IMigrationStep
    {
        private const string DataFolderName = "Data";
        private const string GlobalPanelsName = "Global panels";
        private const string MailTemplatesName = "Mail templates";
        private const string SystemLinksName = "System links";
        private const string UserTagsName = "User tags";

        private readonly IContentTypeService _contentTypeService;
        private readonly IContentService _contentService;

        public UpdateDataFolderNamesMigrationInstallationStep(IContentTypeService contentTypeService, IContentService contentService)
        {
            _contentTypeService = contentTypeService;
            _contentService = contentService;
        }

        ExecutionResult IMigrationStep.Execute()
        {
            var dataFolderContentType = _contentTypeService.GetContentType(CoreInstallationConstants.DocumentTypeAliases.DataFolder);
            var dataFolderPage = _contentService.GetContentOfContentType(dataFolderContentType.Id).First();

            UpdateOldName(dataFolderPage, CoreInstallationConstants.ContentDefaultName.DataFolder, DataFolderName);

            var children = dataFolderPage.Children().ToList();
            UpdateOldName(
                children,
                PanelsInstallationConstants.DocumentTypeAliases.GlobalPanelFolder,
                PanelsInstallationConstants.ContentDefaultName.GlobalPanelFolder,
                GlobalPanelsName);
            UpdateOldName(
                children,
                MailInstallationConstants.DocumentTypeAliases.MailTemplatesFolder,
                MailInstallationConstants.ContentDefaultName.MailTemplatesFolder,
                MailTemplatesName);
            UpdateOldName(
                children,
                NavigationInstallationConstants.DocumentTypeAliases.SystemLinkFolder,
                NavigationInstallationConstants.ContentDefaultName.SystemLinkFolder,
                SystemLinksName);
            UpdateOldName(
                children,
                TaggingInstallationConstants.DocumentTypeAliases.UserTagFolder,
                TaggingInstallationConstants.ContentDefaultName.UserTagFolder,
                UserTagsName);

            return ExecutionResult.Success;
        }

        public void Undo()
        {
            throw new System.NotImplementedException();
        }

        private void UpdateOldName(IContent content, string oldContentName, string newContentName)
        {
            if (!content.Name.Equals(oldContentName)) return;

            content.Name = newContentName;
            _contentService.SaveAndPublishWithStatus(content);
        }

        private void UpdateOldName(List<IContent> contents, string documentTypeAlias, string oldContentName, string newContentName)
        {
            var updateContent = contents.Find(content => content.ContentType.Alias.Equals(documentTypeAlias));
            if (updateContent == null) return;

            UpdateOldName(updateContent, oldContentName, newContentName);
        }
    }
}
