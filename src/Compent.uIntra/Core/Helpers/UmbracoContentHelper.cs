using System.Linq;
using Compent.uIntra.Core.ApplicationSettings;
using uIntra.Core.User;
using Umbraco.Core.Models;

namespace Compent.uIntra.Core.Helpers
{
    public class UmbracoContentHelper : IUmbracoContentHelper
    {
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IuIntraApplicationSettings _applicationSettings;

        public UmbracoContentHelper(IIntranetUserService<IIntranetUser> intranetUserService, IuIntraApplicationSettings applicationSettings)
        {
            _intranetUserService = intranetUserService;
            _applicationSettings = applicationSettings;
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
    }
}