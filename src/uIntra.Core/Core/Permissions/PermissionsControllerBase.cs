using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using LanguageExt;
using Uintra.Core.Extensions;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.Permissions.Models;
using Uintra.Core.Permissions.TypeProviders;
using Uintra.Core.TypeProviders;
using Uintra.Core.User;
using Umbraco.Web.WebApi;
using static LanguageExt.Prelude;

namespace Uintra.Core.Permissions
{    
    public abstract class PermissionsControllerBase : UmbracoAuthorizedApiController
    {
        private readonly IPermissionsService _actionPermissionsService;
        private readonly IIntranetMemberGroupProvider _intranetMemberGroupProvider;
        private readonly IActivityTypeProvider _resourceTypeProvider;
        private readonly IPermissionActionTypeProvider _intranetActionTypeProvider;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;

        protected PermissionsControllerBase(
            IIntranetMemberGroupProvider intranetMemberGroupProvider,
            IPermissionsService actionPermissionsService,
            IActivityTypeProvider resourceTypeProvider,
            IPermissionActionTypeProvider intranetActionTypeProvider,
            IIntranetMemberService<IIntranetMember> intranetMemberService)
        {
            _intranetMemberGroupProvider = intranetMemberGroupProvider;
            _actionPermissionsService = actionPermissionsService;
            _resourceTypeProvider = resourceTypeProvider;
            _intranetActionTypeProvider = intranetActionTypeProvider;
            _intranetMemberService = intranetMemberService;
        }

        [HttpGet]
        public GroupPermissionsViewModel Get(int memberGroupId)
        {
            var isSuperUser = _intranetMemberService.IsCurrentMemberSuperUser;
            var memberGroup = _intranetMemberGroupProvider[memberGroupId];

            var permissions = _actionPermissionsService
                .GetForGroup(memberGroup)
                .Where(i => i.SettingValues.IsEnabled || isSuperUser)
                .Map<IEnumerable<PermissionViewModel>>()
                .OrderBy(i => i.ResourceTypeId);

            var model = new GroupPermissionsViewModel()
            {
                IsSuperUser = isSuperUser,
                Permissions = permissions,
                MemberGroup = memberGroup.Map<MemberGroupViewModel>()
            };

            return model;
        }

        [HttpPost]
        public Unit Save(PermissionUpdateViewModel update)
        {
            var settingIdentity = PermissionSettingIdentity.Of(
                _intranetActionTypeProvider[update.ActionTypeId],
                _resourceTypeProvider[update.ResourceTypeId]);
            var settingValue = PermissionSettingValues.Of(update.Allowed, update.Enabled);
            var targetGroup = _intranetMemberGroupProvider[update.IntranetMemberGroupId];

            var mappedUpdate = PermissionUpdateModel.Of(targetGroup, settingValue, settingIdentity);
            _actionPermissionsService.Save(mappedUpdate);

            return unit;
        }
    }
}
