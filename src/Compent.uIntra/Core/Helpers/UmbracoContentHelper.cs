using System;
using System.Linq;
using Compent.uIntra.Core.ApplicationSettings;
using uIntra.Core;
using uIntra.Core.User;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Compent.uIntra.Core.Helpers
{
    public class UmbracoContentHelper : IUmbracoContentHelper
    {
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IuIntraApplicationSettings _applicationSettings;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;

        public UmbracoContentHelper(IIntranetUserService<IIntranetUser> intranetUserService, IuIntraApplicationSettings applicationSettings, UmbracoHelper umbracoHelper, IDocumentTypeAliasProvider documentTypeAliasProvider)
        {
            _intranetUserService = intranetUserService;
            _applicationSettings = applicationSettings;
            _umbracoHelper = umbracoHelper;
            _documentTypeAliasProvider = documentTypeAliasProvider;
        }

        public bool IsContentAvailable(IPublishedContent publishedContent)
        {
            if (_intranetUserService.GetCurrentUser().Role.Name == IntranetRolesEnum.WebMaster.ToString())
            {
                return true;
            }

            if (_applicationSettings.NotWebMasterRoleDisabledDocumentTypes.Contains(publishedContent.DocumentTypeAlias))
            {
                return false;
            }

            return true;
        }

        public bool IsForContentPage(Guid id)
        {
            return _umbracoHelper.TypedContent(id)?.DocumentTypeAlias == _documentTypeAliasProvider.GetContentPage();
        }
    }
}