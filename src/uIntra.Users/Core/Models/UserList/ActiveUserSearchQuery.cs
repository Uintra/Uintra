using System;
using LanguageExt;

namespace Uintra.Users.UserList
{
    public class ActiveUserSearchQuery
    {
        public string Text { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public string OrderingString { get; set; }
        public Option<Guid> GroupId { get; set; }
    }
}