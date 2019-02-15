namespace Uintra.Core.Permissions.Models
{
    public class PermissionUpdateModel
    {
        public int IntranetMemberGroupId { get; set; }
        public int ActionId { get; set; }
        public int? ActivityTypeId { get; set; }
        public bool Allowed { get; set; }
        public bool Enabled { get; set; }
    }
}
