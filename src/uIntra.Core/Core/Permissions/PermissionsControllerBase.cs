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
        private readonly IPermissionsManagementService _permissionsManagementService;
        private readonly IIntranetMemberGroupProvider _intranetMemberGroupProvider;
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly IIntranetActionTypeProvider _intranetActionTypeProvider;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;

        protected PermissionsControllerBase(
            IIntranetMemberGroupProvider intranetMemberGroupProvider,
            IPermissionsManagementService permissionsManagementService,
            IActivityTypeProvider activityTypeProvider,
            IIntranetActionTypeProvider intranetActionTypeProvider,
            IIntranetMemberService<IIntranetMember> intranetMemberService)
        {
            _intranetMemberGroupProvider = intranetMemberGroupProvider;
            _permissionsManagementService = permissionsManagementService;
            _activityTypeProvider = activityTypeProvider;
            _intranetActionTypeProvider = intranetActionTypeProvider;
            _intranetMemberService = intranetMemberService;
        }

        [HttpGet]
        public GroupPermissionsViewModel Get(int memberGroupId)
        {
            var isSuperUser = _intranetMemberService.IsCurrentMemberSuperUser;
            var memberGroup = _intranetMemberGroupProvider[memberGroupId];
            var permissions = _permissionsManagementService
                .GetGroupManagement(memberGroup)
                .Where(i => i.SettingValues.IsAllowed || isSuperUser)
                .Map<IEnumerable<PermissionViewModel>>();

            var model = new GroupPermissionsViewModel()
            {
                IsSuperUser = isSuperUser,
                Permissions = permissions,
                MemberGroup = memberGroup.Map<MemberGroupViewModel>()
            };

            return model;
        }

        [HttpPost]
        public Unit Save(PermissionUpdateModel update)
        {
            var settingIdentity = PermissionSettingIdentity.Of(
                _intranetActionTypeProvider[update.ActionId],
                update.ActivityTypeId.ToOption().Map(activityTypeId => _activityTypeProvider[activityTypeId]));
            var settingValue = PermissionSettingValues.Of(update.Allowed, update.Enabled);
            var targetGroup = _intranetMemberGroupProvider[update.IntranetMemberGroupId];

            var mappedUpdate = PermissionManagementModel.Of(settingIdentity, settingValue, targetGroup);

            _permissionsManagementService.Save(mappedUpdate);

            return unit;
        }
    }
}
