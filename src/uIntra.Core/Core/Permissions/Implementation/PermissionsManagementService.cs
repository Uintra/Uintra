using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using LanguageExt;
using Uintra.Core.Extensions;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.Permissions.Models;
using static LanguageExt.Prelude;

namespace Uintra.Core.Permissions.Implementation
{
    public class PermissionsManagementService : IPermissionsManagementService
    {
        private readonly IBasePermissionsService _actionPermissionsService;
        private readonly IActivityTypePermissionsService _activityTypePermissionsService;
        private readonly IPermissionSettingsSchema _permissionSettingsSchema;

        public PermissionsManagementService(
            IBasePermissionsService actionPermissionsService,
            IActivityTypePermissionsService activityTypePermissionsService,
            IPermissionSettingsSchema permissionSettingsSchema)
        {
            _actionPermissionsService = actionPermissionsService;
            _activityTypePermissionsService = activityTypePermissionsService;
            _permissionSettingsSchema = permissionSettingsSchema;
        }

        [HttpGet]
        public IEnumerable<PermissionManagementModel> GetGroupManagement(IntranetMemberGroup group)
        {
            var basePerms = _actionPermissionsService
                .GetAll()
                .Where(perm => perm.Group == group);

            var activityTypePerms = _activityTypePermissionsService.GetAll()
                .ToDictionary(perm => perm.BasePermissionId);

            var storedPerms = basePerms
                .Select(basePerm => PermissionManagementModel.Of(
                    basePerm,
                    activityTypePerms.ItemOrNone(basePerm.Id),
                    group))
                .ToDictionary(managementModel => managementModel.SettingIdentity);

            var settings = _permissionSettingsSchema.Settings
                .Select(settingIdentity => storedPerms
                    .ItemOrNone(settingIdentity)
                    .IfNone(() => _permissionSettingsSchema.GetDefault(settingIdentity, group)));

            return settings;
        }

        [HttpPatch]
        public Unit Save(PermissionManagementModel update)
        {
            var basePermUpdate = BasePermissionUpdateModel.Of(update.Group, update.SettingValues, update.SettingIdentity.ActionType);

            var basePerm = _actionPermissionsService.Save(basePermUpdate);

            var activityTypePerm = update.SettingIdentity.ActivityType
                .Map(activityType => ActivityTypePermissionCreateModel.Of(basePerm.Id, activityType));

            activityTypePerm.IfSome(_activityTypePermissionsService.Save);

            return unit;
        }
    }
}