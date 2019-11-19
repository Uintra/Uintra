using System;

namespace Uintra20.Features.User.Models
{
    public class MemberViewModel
    {
        public Guid Id { get; set; }
        public string DisplayedName { get; set; }
        public string Photo { get; set; }
        public int? PhotoId { get; set; }
        public string Email { get; set; }
        public string LoginName { get; set; }
        public bool Inactive { get; set; }
    }
}