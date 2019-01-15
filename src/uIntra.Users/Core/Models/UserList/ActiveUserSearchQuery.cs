using System;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Uintra.Users.UserList
{
    public class ActiveUserSearchQuery
    {
        public string Text { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public string OrderingString { get; set; }
        public int OrderingDirection { get; set; }
        public Option<Guid> GroupId { get; set; } = None;
    }
}