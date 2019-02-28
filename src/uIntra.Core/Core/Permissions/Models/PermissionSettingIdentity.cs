using System;
using LanguageExt;

namespace Uintra.Core.Permissions.Models
{
    public struct PermissionSettingIdentity : IEquatable<PermissionSettingIdentity>
    {
        public Enum ResourceType { get; }
        public Enum ActionType { get; }

        public PermissionSettingIdentity(Enum actionType, Enum resourceType)
        {
            ResourceType = resourceType;
            ActionType = actionType;
        }

        public static PermissionSettingIdentity Of(Enum actionType, Enum resourceType) =>
            new PermissionSettingIdentity(actionType, resourceType);

        public bool Equals(PermissionSettingIdentity other)
        {
            return Equals(ResourceType, other.ResourceType) && Equals(ActionType, other.ActionType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is PermissionSettingIdentity other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((ResourceType != null ? ResourceType.GetHashCode() : 0) * 397) ^ (ActionType != null ? ActionType.GetHashCode() : 0);
            }
        }
    }

    public struct PermissionSettingSchema : IEquatable<PermissionSettingSchema>
    {
        public PermissionSettingIdentity SettingIdentity { get; }
        public Option<Enum> ParentActionType { get; }

        public PermissionSettingSchema(PermissionSettingIdentity settingIdentity, Option<Enum> parentActionType)
        {
            SettingIdentity = settingIdentity;
            ParentActionType = parentActionType;
        }

        public static PermissionSettingSchema Of(PermissionSettingIdentity settingIdentity, Option<Enum> parentActionType) =>
            new PermissionSettingSchema(settingIdentity, parentActionType);

        public bool Equals(PermissionSettingSchema other)
        {
            return SettingIdentity.Equals(other.SettingIdentity) && Equals(ParentActionType, other.ParentActionType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is PermissionSettingSchema other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (SettingIdentity.GetHashCode() * 397) ^ (ParentActionType != null ? ParentActionType.GetHashCode() : 0);
            }
        }
    }
}