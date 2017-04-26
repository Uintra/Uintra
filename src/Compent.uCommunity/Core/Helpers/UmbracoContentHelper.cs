using System.Linq;
using Compent.uCommunity.Core.ApplicationSettings;
using uCommunity.Core.User;
using Umbraco.Core.Models;

namespace Compent.uCommunity.Core.Helpers
{
    public class UmbracoContentHelper : IUmbracoContentHelper
    {
        private readonly IIntranetUserService _intranetUserService;
        private readonly IUcommunityApplicationSettings _applicationSettings;

        public UmbracoContentHelper(IIntranetUserService intranetUserService, IUcommunityApplicationSettings applicationSettings)
        {
            _intranetUserService = intranetUserService;
            _applicationSettings = applicationSettings;
        }

        public bool IsContentAvailable(IPublishedContent publishedContent)
        {
            if (_intranetUserService.GetCurrentUser().Role == IntranetRolesEnum.WebMaster)
            {
                return true;
            }

            if (_applicationSettings.NotWebMasterRoleDisabledDocumentTypes.Contains(publishedContent.DocumentTypeAlias))
            {
                return false;
            }

            return true;
        }
    }
}