using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Uintra.Core.Extensions;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.Permissions.Models;
using Uintra.Core.Permissions.TypeProviders;
using Uintra.Core.TypeProviders;
using static LanguageExt.Prelude;

namespace Uintra.Core.Permissions.Implementation
{
    public class PermissionSettingsSchema : IPermissionSettingsSchema
    {
        private const bool GlobalIsAllowedDefault = true;
        private const bool GlobalIsEnabledDefault = true;

        private readonly Dictionary<PermissionSettingIdentity, PermissionSettingValues> _defaultOverrides =
            new Dictionary<PermissionSettingIdentity, PermissionSettingValues>();

        public PermissionSettingIdentity[] Settings { get; }

        public PermissionSettingsSchema(
            IIntranetActionTypeProvider actionTypeProvider,
            IActivityTypeProvider activityTypeProvider)
        {

            Settings = BuildSettings(actionTypeProvider.All, actionTypeProvider.ActivityActions, activityTypeProvider.All);
        }


        public BasePermissionModel GetDefault(PermissionSettingIdentity settingIdentity, IntranetMemberGroup group) =>
            _defaultOverrides
                .ItemOrNone(settingIdentity)
                .IfNone(() => PermissionSettingValues.Of(GlobalIsAllowedDefault, GlobalIsEnabledDefault))
                .Apply(settingValues => BasePermissionModel.Of(settingIdentity, settingValues, group));

        public static PermissionSettingIdentity[] BuildSettings(
            IEnumerable<Enum> allActions,
            IEnumerable<Enum> activityActions,
            IEnumerable<Enum> activityTypes)
        {
            var activityActionsArray = activityActions.ToArray();
            var baseActions = allActions.Except(activityActionsArray);

            var baseSettings = baseActions
                .Select(baseAction => PermissionSettingIdentity.Of(baseAction, None));

            var activitySettings = activityActionsArray
                .CartesianProduct(activityTypes)
                .Select(activitySettingPair => PermissionSettingIdentity.Of(activitySettingPair.Item1, activitySettingPair.Item2));

            return baseSettings.Concat(activitySettings).ToArray();
        }
    }
}