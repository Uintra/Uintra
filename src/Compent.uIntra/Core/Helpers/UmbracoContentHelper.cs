using System.Linq;
using Compent.uIntra.Core.ApplicationSettings;
using uCommunity.Core.User;
using uCommunity.Users.Core;
using Umbraco.Core.Models;

namespace Compent.uIntra.Core.Helpers
{
    public class UmbracoContentHelper : IUmbracoContentHelper
    {
        private readonly IIntranetUserService<IntranetUser> _intranetUserService;
        private readonly IUcommunityApplicationSettings _applicationSettings;

        public UmbracoContentHelper(IIntranetUserService<IntranetUser> intranetUserService, IUcommunityApplicationSettings applicationSettings)
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