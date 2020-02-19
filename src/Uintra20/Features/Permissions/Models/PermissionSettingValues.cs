namespace Uintra20.Features.Permissions.Models
{
    public class PermissionSettingValues
    {
        public bool IsAllowed { get; }
        public bool IsEnabled { get; }

        public PermissionSettingValues(bool isAllowed, bool isEnabled)
        {
            IsAllowed = isAllowed;
            IsEnabled = isEnabled;
        }
    }
}