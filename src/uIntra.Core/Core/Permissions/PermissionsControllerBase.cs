using System.Collections.Generic;
using LanguageExt;
using Uintra.Core.Extensions;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.Permissions.Models;
using Uintra.Core.Permissions.TypeProviders;
using Uintra.Core.TypeProviders;
using Umbraco.Web.Mvc;
using static LanguageExt.Prelude;

namespace Uintra.Core.Permissions
{    
    public abstract class PermissionsControllerBase : SurfaceController
    {
        private readonly IPermissionsManagementService _permissionsManagementService;
        private readonly IIntranetMemberGroupProvider _intranetMemberGroupProvider;
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly IIntranetActionTypeProvider _intranetActionTypeProvider;

        protected PermissionsControllerBase(
            IIntranetMemberGroupProvider intranetMemberGroupProvider,
            IPermissionsManagementService permissionsManagementService,
            IActivityTypeProvider activityTypeProvider,
            IIntranetActionTypeProvider intranetActionTypeProvider)
        {
            _intranetMemberGroupProvider = intranetMemberGroupProvider;
            _permissionsManagementService = permissionsManagementService;
            _activityTypeProvider = activityTypeProvider;
            _intranetActionTypeProvider = intranetActionTypeProvider;
        }

        public IEnumerable<PermissionViewModel> Get(int memberGroupId)
        {
            var memberGroup = _intranetMemberGroupProvider[memberGroupId];
            var settings = _permissionsManagementService
                .GetGroupManagement(memberGroup)
                .Map<IEnumerable<PermissionViewModel>>();

            return settings;
        }

        public Unit Save(PermissionViewModel update)
        {
            var mappedUpdate = PermissionManagementModel.Of(
                PermissionSettingIdentity.Of(
                    _intranetActionTypeProvider[update.ActionId],
                    update.ActivityTypeId.ToOption().Map(activityTypeId => _activityTypeProvider[activityTypeId])),
                PermissionSettingValues.Of(update.IsAllowed, update.IsEnabled),
                _intranetMemberGroupProvider[update.IntranetMemberGroupId]
            );

            _permissionsManagementService.Save(mappedUpdate);

            return unit;
        }
    }
}
