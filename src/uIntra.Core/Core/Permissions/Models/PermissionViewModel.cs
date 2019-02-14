namespace Uintra.Core.Permissions.Models
{
    public class PermissionViewModel
    {
        public int IntranetMemberGroupId { get; set; }
        public int ActionId { get; set; }
        public int? ActivityTypeId { get; set; }
        public bool IsAllowed { get; set; }
        public bool IsEnabled { get; set; }
    }
}
