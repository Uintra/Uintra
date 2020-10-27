using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Controls.LightboxGallery;
using Uintra.Core.Member.Models;
using Uintra.Features.CentralFeed.Models.GroupFeed;
using Uintra.Features.Likes;
using Uintra.Features.Likes.Models;
using Uintra.Features.LinkPreview.Models;
using Uintra.Features.Links.Models;
using Uintra.Features.Location.Models;

namespace Uintra.Core.Activity.Models
{
    public class IntranetActivityPreviewModelBase : ILikeable, IHaveLightboxPreview
    {
        public Guid Id { get; set; }
        public bool CanEdit { get; set; }
        public bool IsPinned { get; set; }
        public bool IsPinActual { get; set; }
        public bool CurrentMemberSubscribed { get; set; }
        public string Title { get; set; }
        public MemberViewModel Owner { get; set; }
        public IEnumerable<string> Dates { get; set; } = Enumerable.Empty<string>();
        public IActivityLinks Links { get; set; }
        public ActivityLocation Location { get; set; }
        public bool IsReadOnly { get; set; }
        public string Type { get; set; }
        public Enum ActivityType { get; set; }
        public IEnumerable<LikeModel> Likes { get; set; }
        public bool LikedByCurrentUser { get; set; }
        public int CommentsCount { get; set; }
        public LightboxPreviewModel MediaPreview { get; set; }
        public string Description { get; set; }
        public GroupInfo? GroupInfo { get; set; }
        public bool IsGroupMember { get; set; }
        public int? LinkPreviewId { get; set; }
        public LinkPreviewModel LinkPreview { get; set; }
    }
}