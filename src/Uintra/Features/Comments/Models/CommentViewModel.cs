using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Member.Models;
using Uintra.Features.Likes.Models;
using Uintra.Features.LinkPreview.Models;
using Uintra.Features.Links.Models;

namespace Uintra.Features.Comments.Models
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }
        public MemberViewModel Creator { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime? ModifyDate { get; set; }
        public string CreatedDate { get; set; }
        public string ModifyDate { get; set; }
        public string Text { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool IsReply { get; set; }
        public string ElementOverviewId { get; set; }
        public string CommentViewId { get; set; }
        public IEnumerable<CommentViewModel> Replies { get; set; } = Enumerable.Empty<CommentViewModel>();
        public UintraLinkModel CreatorProfileUrl { get; set; }
        public LinkPreviewModel LinkPreview { get; set; }
        public LikesViewModel LikeModel { get; set; }
        public bool LikedByCurrentUser { get; set; }
        public IEnumerable<LikeModel> Likes { get; set; } = Enumerable.Empty<LikeModel>();
    }
}