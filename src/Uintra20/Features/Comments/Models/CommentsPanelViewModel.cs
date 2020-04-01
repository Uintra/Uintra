using System;
using System.Collections.Generic;
using Uintra20.Core.UbaselineModels.RestrictedNode;

namespace Uintra20.Features.Comments.Models
{
    public class CommentsPanelViewModel : UintraRestrictedNodeViewModel
    {
        public IEnumerable<CommentViewModel> Comments { get; set; }
        public Enum CommentsType { get; set; }
        public Guid? EntityId { get; set; }
        public bool IsGroupMember { get; set; }
    }
}