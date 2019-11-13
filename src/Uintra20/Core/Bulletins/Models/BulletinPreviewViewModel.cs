using System;
using Uintra20.Core.Links;
using Uintra20.Core.User;

namespace Uintra20.Core.Bulletins
{
    public class BulletinPreviewViewModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public MemberViewModel Owner { get; set; }
        public Enum ActivityType { get; set; }
        public ActivityLinks Links { get; set; }
    }
}