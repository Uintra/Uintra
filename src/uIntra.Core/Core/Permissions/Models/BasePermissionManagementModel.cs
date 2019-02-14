using System;
using LanguageExt;

namespace Uintra.Core.Permissions.Models
{
    public struct PermissionSettingIdentity: IEquatable<PermissionSettingIdentity>
    {
        public Option<Enum> ActivityType { get; }
        public Enum ActionType { get; }

        public PermissionSettingIdentity(Enum actionType, Option<Enum> activityType)
        {
            ActivityType = activityType;
            ActionType = actionType;
        }

        public static PermissionSettingIdentity Of(Enum actionType, Option<Enum> activityType) =>
            new PermissionSettingIdentity(actionType, activityType);

        public bool Equals(PermissionSettingIdentity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ActivityType.Equals(other.ActivityType) && Equals(ActionType, other.ActionType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PermissionSettingIdentity) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (ActivityType.GetHashCode() * 397) ^ (ActionType != null ? ActionType.GetHashCode() : 0);
            }
        }
    }
}