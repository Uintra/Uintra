using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Member.Models;
using Uintra20.Features.Likes;
using Uintra20.Features.Likes.Models;
using Uintra20.Features.Links.Models;
using Uintra20.Features.Location.Models;

namespace Uintra20.Core.Activity.Models
{
    public abstract class IntranetActivityPreviewModelBase : ILikeable
    {
        public Guid Id { get; set; }
        public bool CanEdit { get; set; }
        public bool IsPinned { get; set; }
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
    }
}