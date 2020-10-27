using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Activity.Models;
using Uintra.Core.Controls.LightboxGallery;
using Uintra.Features.Comments.Services;
using Uintra.Features.Likes;
using Uintra.Features.Tagging.UserTags.Models;

namespace Uintra.Features.News.Models
{
    public class NewsViewModel : IntranetActivityViewModelBase
    {
        public Guid OwnerId { get; set; }
        public string Description { get; set; }
        public DateTime? EndPinDate { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime? UnpublishDate { get; set; }
        public IEnumerable<string> Media { get; set; } = Enumerable.Empty<string>();
        public ILikeable LikesInfo { get; set; }
        public ICommentable CommentsInfo { get; set; }
        public LightboxPreviewModel LightboxPreviewModel { get; set; }
        public IEnumerable<UserTag> Tags { get; set; } = Enumerable.Empty<UserTag>();
        public IEnumerable<UserTag> AvailableTags { get; set; } = Enumerable.Empty<UserTag>();
    }
}