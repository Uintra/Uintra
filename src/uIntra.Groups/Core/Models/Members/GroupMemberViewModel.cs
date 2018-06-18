
namespace Uintra.Groups
{
    public class GroupMemberViewModel
    {
        public IGroupMember GroupMember { get; set; }
        public bool IsGroupAdmin { get; set; }
        public bool CanUnsubscribe { get; set; }
    }
}