using System;
using System.Collections.Generic;
using Uintra.Core.UbaselineModels.RestrictedNode;

namespace Uintra.Features.Comments.Models
{
    public class CommentsPanelViewModel : UintraRestrictedNodeViewModel
    {
        public IEnumerable<CommentViewModel> Comments { get; set; }
        public Enum CommentsType { get; set; }
        public Guid? EntityId { get; set; }
        public bool IsGroupMember { get; set; }
    }
}