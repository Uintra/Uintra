using System.Linq;
using System.Web.Mvc;
using uIntra.Notification;
using Uintra.Core;
using Uintra.Notification;
using Uintra.Notification.Configuration;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Compent.Uintra.Core.Updater.Migrations._0._3._1._0.Steps
{
    public class RemoveMailTeplatesFolderStep : IMigrationStep
    {
        private readonly NotificationSettingsService _notificationSettingsService;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IContentService _contentService;

        public RemoveMailTeplatesFolderStep(
            NotificationSettingsService notificationSettingsService,
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            UmbracoHelper umbracoHelper,
            IContentService contentService)
        {
            _notificationSettingsService = notificationSettingsService;
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _umbracoHelper = umbracoHelper;
            _contentService = contentService;
        }

        public ExecutionResult Execute()
        {
            var mailTemplateXpath = XPathHelper.GetXpath(
                _documentTypeAliasProvider.GetDataFolder(),
                _documentTypeAliasProvider.GetMailTemplateFolder(),
                _documentTypeAliasProvider.GetMailTemplate());

            var templates = _umbracoHelper.TypedContentAtXPath(mailTemplateXpath);

            var mailTemplateFolderXpath = XPathHelper.GetXpath(_documentTypeAliasProvider.GetDataFolder(), _documentTypeAliasProvider.GetMailTemplateFolder());
            
            if (!templates.Any())
            {
                var folder = _umbracoHelper.TypedContentSingleAtXPath(mailTemplateFolderXpath);
                if (folder != null)
                {
                    var folderContent = _contentService.GetById(folder.Id);
                    _contentService.Delete(folderContent);
                }
            }

            return ExecutionResult.Success;
        }

        public void Undo()
        {

        }
    }
}