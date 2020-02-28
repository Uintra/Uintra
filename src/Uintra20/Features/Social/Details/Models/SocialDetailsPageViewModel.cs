using System;
using System.Collections.Generic;
using System.Linq;
using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.Property;
using Uintra20.Features.Comments.Models;
using Uintra20.Features.Likes.Models;
using Uintra20.Features.Social.Models;
using Uintra20.Features.Tagging.UserTags.Models;

namespace Uintra20.Features.Social.Details.Models
{
    public class SocialDetailsPageViewModel : NodeViewModel
    {
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
        public PageSettingsCompositionViewModel PageSettings { get; set; }
        public SocialExtendedViewModel Details { get; set; }
        public IEnumerable<UserTag> Tags { get; set; } = Enumerable.Empty<UserTag>();
        public IEnumerable<LikeModel> Likes { get; set; } = Enumerable.Empty<LikeModel>();
        public IEnumerable<CommentViewModel> Comments = Enumerable.Empty<CommentViewModel>();
        public bool LikedByCurrentUser { get; set; }
        public Guid? GroupId { get; set; }
        public bool RequiresGroupHeader { get; set; }
        public bool CanEdit { get; set; }
        public bool CanView { get; set; }
    }
}