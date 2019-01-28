using System;

namespace Uintra.Users.UserList
{
    public class ActiveUserSearchQueryModel
    {
        public string Text { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public string OrderingString { get; set; }
        public Guid? GroupId { get; set; }
    }
}