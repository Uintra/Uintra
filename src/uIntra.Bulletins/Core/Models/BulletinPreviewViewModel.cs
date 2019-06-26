using System;
using Uintra.Core.Links;
using Uintra.Core.User;

namespace Uintra.Bulletins
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