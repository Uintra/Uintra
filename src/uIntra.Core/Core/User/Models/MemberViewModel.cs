using System;

namespace Uintra.Core.User
{
    public class MemberViewModel
    {
        public Guid Id { get; set; }
        public string DisplayedName { get; set; }
        public string Photo { get; set; }
        public string Email { get; set; }
        public string LoginName { get; set; }
        public bool Inactive { get; set; }
    }
}
