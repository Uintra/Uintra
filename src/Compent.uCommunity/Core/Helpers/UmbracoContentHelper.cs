using System;
using System.Collections.Generic;
using System.Linq;
using Compent.uCommunity.Core.Constants;
using uCommunity.Core;
using uCommunity.Core.User;
using Umbraco.Core.Models;

namespace Compent.uCommunity.Core.Helpers
{
    public class UmbracoContentHelper : IUmbracoContentHelper
    {
        private readonly IEnumerable<string> notWebMasterRoleDisabledDocumentTypes;

        private readonly IIntranetUserService _intranetUserService;

        public UmbracoContentHelper(IIntranetUserService intranetUserService)
        {
            _intranetUserService = intranetUserService;

            notWebMasterRoleDisabledDocumentTypes = AppSettingHelper.GetAppSetting<string>(AppSettingConstants.NotWebMasterRoleDisabledDocumentTypes)
                .Split(new[] { "," }, StringSplitOptions.None)
                .Select(el => el.Trim());
        }

        public bool IsContentAvailable(IPublishedContent publishedContent)
        {
            if (_intranetUserService.GetCurrentUser().Role == IntranetRolesEnum.WebMaster)
            {
                return true;
            }

            if (notWebMasterRoleDisabledDocumentTypes.Contains(publishedContent.DocumentTypeAlias))
            {
                return false;
            }

            return true;
        }
    }
}