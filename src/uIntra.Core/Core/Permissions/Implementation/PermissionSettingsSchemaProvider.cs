using System.Collections.Generic;
using Uintra.Core.Extensions;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.Permissions.Models;
using static Compent.Extensions.Trees.TreeExtensions;
using static Uintra.Core.Core.Permissions.Implementation.PermissionSettingsSchemaProviderFunctions;
using static Uintra.Core.Permissions.Models.ResourceToActionRelation;
using static Uintra.Core.Permissions.PermissionActionEnum;
using static Uintra.Core.Permissions.PermissionResourceTypeEnum;

namespace Uintra.Core.Permissions.Implementation
{
    public class PermissionSettingsSchemaProvider : IPermissionSettingsSchemaProvider
    {
        private const bool GlobalIsAllowedDefault = false;
        private const bool GlobalIsEnabledDefault = true;

        protected readonly Dictionary<PermissionSettingIdentity, PermissionSettingValues> SettingsOverrides =
            new Dictionary<PermissionSettingIdentity, PermissionSettingValues>();

        protected  ResourceToActionRelation[] BaseSettingsSchema =
        {
            Of(Bulletins,
                Tree(View,
                    Tree(Create),
                    Tree(Edit,
                        Tree(Delete)),
                    Tree(EditOther,
                        Tree(DeleteOther))
                    )),
            Of(Events,
                Tree(View,
                    Tree(Create),
                    Tree(Edit),
                    Tree(EditOther,
                        Tree(EditOwner))
                    )),
            Of(News,
                Tree(View,
                    Tree(Create),
                    Tree(Edit),
                    Tree(EditOther,
                        Tree(EditOwner))
                    )),
            Of(Groups,
                Tree(Create),
                Tree(Edit),
                Tree(EditOther))
        };

        public PermissionSettingValues DefaultSettingsValues =>
            PermissionSettingValues.Of(GlobalIsAllowedDefault, GlobalIsEnabledDefault);

        public PermissionSettingSchema[] Settings { get; }

        public PermissionSettingsSchemaProvider()
        {
            Settings = BuildSettings(BaseSettingsSchema);
        }

        public virtual PermissionSettingValues GetDefault(PermissionSettingIdentity settingIdentity) =>
            SettingsOverrides
                .ItemOrNone(settingIdentity)
                .IfNone(() => PermissionSettingValues.Of(GlobalIsAllowedDefault, GlobalIsEnabledDefault));     
    }
}