using System;
using Uintra.Core.User;

namespace Uintra.Groups
{
    public class GroupViewModel
    {
        public Guid Id { get; set; }
        public bool HasImage { get; set; }
        public string GroupUrl { get; set; }
        public string GroupImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IIntranetUser Creator { get; set; }
        public bool IsMember { get; set; }
        public int MembersCount { get; set; }
    }
}