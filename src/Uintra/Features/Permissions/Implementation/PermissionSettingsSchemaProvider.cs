﻿using System.Collections.Generic;
using System.Linq;
using Uintra.Features.Permissions.Interfaces;
using Uintra.Features.Permissions.Models;
using Uintra.Infrastructure.Extensions;
using static Compent.Extensions.Trees.TreeExtensions;
using static Uintra.Features.Permissions.Implementation.PermissionSettingsSchemaProviderFunctions;
using static Uintra.Features.Permissions.Models.ResourceToActionRelation;
using static Uintra.Features.Permissions.PermissionActionEnum;

namespace Uintra.Features.Permissions.Implementation
{
    public class PermissionSettingsSchemaProvider : IPermissionSettingsSchemaProvider
    {
        private const bool GlobalIsAllowedDefault = false;
        private const bool GlobalIsEnabledDefault = true;

        protected readonly Dictionary<PermissionSettingIdentity, PermissionSettingValues> SettingsOverrides =
            new Dictionary<PermissionSettingIdentity, PermissionSettingValues>();

        protected ResourceToActionRelation[] BaseSettingsSchema =
        {
            Of(PermissionResourceTypeEnum.Social,
                Tree(View,
                    Tree(Create,
                        Tree(Edit,
                            Tree(Delete))),
                    Tree(EditOther,
                        Tree(DeleteOther))
                )),
            Of(PermissionResourceTypeEnum.Events,
                Tree(View,
                    Tree(Create,
                        Tree(Edit,
                            Tree(Hide))),
                    Tree(EditOther,
                        Tree(HideOther),
                        Tree(EditOwner)),
                    Tree(CanPin)
                )),
            Of(PermissionResourceTypeEnum.News,
                Tree(View,
                    Tree(Create, 
                        Tree(Edit)),
                    Tree(EditOther,
                        Tree(EditOwner)),
                    Tree(CanPin)
                )),
            Of(PermissionResourceTypeEnum.Groups,
                Tree(Create),
                Tree(EditOther,
                    Tree(HideOther)))
        };

        public virtual PermissionSettingValues DefaultSettingsValues =>
            new PermissionSettingValues(GlobalIsAllowedDefault, GlobalIsEnabledDefault);

        public virtual PermissionSettingSchema[] Settings { get; }

        public virtual ILookup<PermissionSettingIdentity, PermissionSettingIdentity>
            SettingsByParentSettingIdentityLookup { get; }

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
            SettingsOverrides.ItemOrDefault(settingIdentity)
            ?? new PermissionSettingValues(GlobalIsAllowedDefault, GlobalIsEnabledDefault);
    }
}