using System.Linq;
using System.Web.Mvc;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Web;
using static Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants.NavigationInstallationConstants;

namespace Compent.Uintra.Core.Updater.Migrations._1._2.Steps
{
    public class ChangeSystemLinksToSharedLinksStep : IMigrationStep
    {
        public ExecutionResult Execute()
        {
            RenameSystemLinkFolder();
            RenameSystemLinkDocumentType();
            
            return ExecutionResult.Success;
        }

        public void Undo()
        {

        }

        private void RenameSystemLinkFolder()
        {
            var contentTypeService = ApplicationContext.Current.Services.ContentTypeService;

            var dataFolderDocType = contentTypeService.GetContentType(DocumentTypeAliases.SystemLinkFolder);
            if (dataFolderDocType is null) return;
          
            dataFolderDocType.Name = DocumentTypeNames.SystemLinkFolder;

            contentTypeService.Save(dataFolderDocType);

            var contentService = DependencyResolver.Current.GetService<IContentService>();

            var folders = contentService.GetContentOfContentType(dataFolderDocType.Id).ToList();

            foreach (var folder in folders)
            {
                folder.Name = DocumentTypeNames.SystemLinkFolder;
            }

            contentService.Save(folders);
        }

        private void RenameSystemLinkDocumentType()
        {

            var contentTypeService = ApplicationContext.Current.Services.ContentTypeService;

            var navigationCompositionType = contentTypeService.GetContentType(
                DocumentTypeAliases.SystemLink);
            if (navigationCompositionType is null) return;

            navigationCompositionType.Name = DocumentTypeNames.SystemLink;

            contentTypeService.Save(navigationCompositionType);
        }
    }
}