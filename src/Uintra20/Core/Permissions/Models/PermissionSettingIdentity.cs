using System;
using LanguageExt;

namespace Uintra20.Core.Permissions.Models
{
    public struct PermissionSettingIdentity : IEquatable<PermissionSettingIdentity>
    {
        public Enum ResourceType { get; }
        public Enum Action { get; }

        public PermissionSettingIdentity(Enum action, Enum resourceType)
        {
            ResourceType = resourceType;
            Action = action;
        }

        public static PermissionSettingIdentity Of(Enum action, Enum resourceType) =>
            new PermissionSettingIdentity(action, resourceType);

        public bool Equals(PermissionSettingIdentity other)
        {
            return Equals(ResourceType, other.ResourceType) && Equals(Action, other.Action);
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
                return ((ResourceType != null ? ResourceType.GetHashCode() : 0) * 397) ^ (Action != null ? Action.GetHashCode() : 0);
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