namespace Uintra20.Features.Permissions.Models
{
    public class PermissionUpdateViewModel
    {
        public int IntranetMemberGroupId { get; set; }
        public int ActionId { get; set; }
        public int ResourceTypeId { get; set; }
        public bool Allowed { get; set; }
        public bool Enabled { get; set; }
    }
}