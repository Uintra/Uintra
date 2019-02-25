using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using LanguageExt;
using Uintra.Core.Extensions;
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
        private readonly IBasePermissionsService _actionPermissionsService;
        private readonly IIntranetMemberGroupProvider _intranetMemberGroupProvider;
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly IPermissionActionTypeProvider _intranetActionTypeProvider;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;

        protected PermissionsControllerBase(
            IIntranetMemberGroupProvider intranetMemberGroupProvider,
            IBasePermissionsService actionPermissionsService,
            IActivityTypeProvider activityTypeProvider,
            IPermissionActionTypeProvider intranetActionTypeProvider,
            IIntranetMemberService<IIntranetMember> intranetMemberService)
        {
            _intranetMemberGroupProvider = intranetMemberGroupProvider;
            _actionPermissionsService = actionPermissionsService;
            _activityTypeProvider = activityTypeProvider;
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
                .Where(i => i.IsEnabled || isSuperUser)
                .Map<IEnumerable<PermissionViewModel>>()
                .OrderBy(i => i.ActivityTypeId ?? int.MaxValue);

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

            var mappedUpdate = BasePermissionUpdateModel.Of(targetGroup, settingValue, settingIdentity);
            _actionPermissionsService.Save(mappedUpdate);

            return unit;
        }
    }
}
