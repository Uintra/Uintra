using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Extensions.Trees;
using Uintra.Core.Extensions;
using Uintra.Core.Permissions.Models;

namespace Uintra.Core.Core.Permissions.Implementation
{
    public static class PermissionSettingsSchemaProviderFunctions
    {

        public static PermissionSettingSchema[] BuildSettings(
            IEnumerable<ResourceToActionRelation> hierarchy) =>
            hierarchy.SelectMany(resourceActions =>
                    resourceActions.Actions.SelectMany(actionHierarchyItem =>
                        BuildTree(resourceActions.Resource, actionHierarchyItem)))
                .ToArray();

        public static IEnumerable<PermissionSettingSchema> BuildTree(Enum resource, ITree<Enum> actionTree)
        {
            var treeWithParents = actionTree
                .WithAttachedParents()
                .Flatten();

            var settings = treeWithParents.Select(hierarchicalItem =>
                PermissionSettingSchema.Of(
                    PermissionSettingIdentity.Of(hierarchicalItem.current, resource),
                    hierarchicalItem.parent));

            return settings;
        }

        public static ILookup<PermissionSettingIdentity, PermissionSettingIdentity> BuildSettingsByParentSettingIdentityLookup(
            IEnumerable<PermissionSettingSchema> settingSchema)=>
            settingSchema
                .Select(setting =>
                    setting.ParentActionType.Map(parentActionType => (
                        parentIdentity: PermissionSettingIdentity.Of(parentActionType, setting.SettingIdentity.ResourceType),
                        childIdentity: setting.SettingIdentity)))
                .Somes()
                .ToLookup(tuple => tuple.parentIdentity, tuple => tuple.childIdentity);
    }
}
