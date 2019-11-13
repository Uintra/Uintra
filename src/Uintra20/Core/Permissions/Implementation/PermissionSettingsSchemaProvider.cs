using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Extensions;
using Uintra20.Core.Permissions.Interfaces;
using Uintra20.Core.Permissions.Models;
using static Compent.Extensions.Trees.TreeExtensions;
using static Uintra20.Core.Permissions.Implementation.PermissionSettingsSchemaProviderFunctions;
using static Uintra20.Core.Permissions.Models.ResourceToActionRelation;
using static Uintra20.Core.Permissions.PermissionActionEnum;
using static Uintra20.Core.Permissions.PermissionResourceTypeEnum;

namespace Uintra20.Core.Permissions.Implementation
{
    public class PermissionSettingsSchemaProvider : IPermissionSettingsSchemaProvider
    {
        private const bool GlobalIsAllowedDefault = false;
        private const bool GlobalIsEnabledDefault = true;

        protected readonly Dictionary<PermissionSettingIdentity, PermissionSettingValues> SettingsOverrides =
            new Dictionary<PermissionSettingIdentity, PermissionSettingValues>();

        protected ResourceToActionRelation[] BaseSettingsSchema =
        {
            Of(PermissionResourceTypeEnum.Bulletins,
                Tree(View,
                    Tree(Create),
                    Tree(Edit,
                        Tree(Delete)),
                    Tree(EditOther,
                        Tree(DeleteOther))
                    )),
            Of(PermissionResourceTypeEnum.Events,
                Tree(View,
                    Tree(Create),
                    Tree(Edit,
                        Tree(Hide,Tree(CanPin))),
                    Tree(EditOther,
                        Tree(HideOther),
                        Tree(EditOwner))
                    )),
            Of(News,
                Tree(View,
                    Tree(Create),
                    Tree(Edit,Tree(CanPin)),
                    Tree(EditOther,
                        Tree(EditOwner))
                    )),
            Of(PermissionResourceTypeEnum.Groups,
                Tree(Create),
                Tree(EditOther,
                    Tree(HideOther)))
        };

        public virtual PermissionSettingValues DefaultSettingsValues =>
            PermissionSettingValues.Of(GlobalIsAllowedDefault, GlobalIsEnabledDefault);

        public virtual PermissionSettingSchema[] Settings { get; }

        public virtual ILookup<PermissionSettingIdentity, PermissionSettingIdentity> SettingsByParentSettingIdentityLookup { get; }

        public PermissionSettingsSchemaProvider()
        {
            Settings = BuildSettings(BaseSettingsSchema);
            SettingsByParentSettingIdentityLookup = BuildSettingsByParentSettingIdentityLookup(Settings);
        }

        public virtual IEnumerable<PermissionSettingIdentity> GetDescendants(PermissionSettingIdentity parent)
        {
            var children = SettingsByParentSettingIdentityLookup[parent].ToArray();
            return children.Concat(children.SelectMany(GetDescendants));
        }

        public virtual PermissionSettingValues GetDefault(PermissionSettingIdentity settingIdentity) =>
            SettingsOverrides
                .ItemOrNone(settingIdentity)
                .IfNone(() => PermissionSettingValues.Of(GlobalIsAllowedDefault, GlobalIsEnabledDefault));
    }
}