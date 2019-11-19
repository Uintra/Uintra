using System;
using Uintra20.Features.Links.Models;
using Uintra20.Features.User.Models;

namespace Uintra20.Features.Bulletins.Models
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