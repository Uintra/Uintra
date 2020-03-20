using System;

namespace Uintra20.Features.UserList.Models
{
    public class MembersListSearchModel
    {
        public string Text { get; set; }
        public int Page { get; set; }
        public string OrderingString { get; set; }
        public Guid? GroupId { get; set; }
        public bool IsInvite { get; set; }
    }
}