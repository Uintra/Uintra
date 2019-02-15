namespace Uintra.Core.Permissions.Models
{
    public class PermissionViewModel
    {
        public int IntranetMemberGroupId { get; set; }
        public int ActionId { get; set; }
        public string ActionName { get; set; }
        public int? ActivityTypeId { get; set; }
        public string ActivityTypeName { get; set; }
        public bool Allowed { get; set; }
        public bool Enabled { get; set; }
    }
}
