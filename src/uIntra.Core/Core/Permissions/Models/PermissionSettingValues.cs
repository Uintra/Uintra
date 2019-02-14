namespace Uintra.Core.Permissions.Models
{
    public class PermissionSettingValues
    {
        public bool IsAllowed { get; }
        public bool IsEnabled { get; }

        public PermissionSettingValues(bool isAllowed, bool isDisabled)
        {
            IsAllowed = isAllowed;
            IsEnabled = isDisabled;
        }

        public static PermissionSettingValues Of(bool isAllowed, bool isDisabled) =>
            new PermissionSettingValues(isAllowed, isDisabled);
    }
}