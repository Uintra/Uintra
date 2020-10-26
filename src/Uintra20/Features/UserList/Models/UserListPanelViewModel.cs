using System;
using UBaseline.Shared.Node;

namespace Uintra20.Features.UserList.Models
{
    public class UserListPanelViewModel : NodeViewModel
    {
        public MembersRowsViewModel Details { get; set; }
        public Guid? GroupId { get; set; }
    }
}