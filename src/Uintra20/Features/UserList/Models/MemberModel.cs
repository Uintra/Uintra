using Uintra20.Attributes;
using Uintra20.Core.Member.Models;
using Uintra20.Features.Links.Models;

namespace Uintra20.Features.UserList.Models
{
    [UIColumn(0, "Name", "fullName", ColumnType.Name, SupportSorting = true)]
    [UIColumn(1, "Info", "info", ColumnType.Info)]
    //[UIColumn(2, "Button", "button", ColumnType.Button)]
    public class MemberModel
    {
        public string Photo { get; set; }
        public string DisplayedName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public MemberViewModel Member { get; set; }
        public UintraLinkModel ProfileUrl { get; set; }
        public bool IsGroupAdmin { get; set; }
        public bool IsCreator { get; set; }
    }
}