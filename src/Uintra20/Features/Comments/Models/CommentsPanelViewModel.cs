using System;
using System.Collections.Generic;
using UBaseline.Shared.Node;

namespace Uintra20.Features.Comments.Models
{
    public class CommentsPanelViewModel : NodeViewModel
    {
        public IEnumerable<CommentViewModel> Comments { get; set; }
        public Enum ActivityType { get; set; }
        public Guid EntityId { get; set; }
    }
}