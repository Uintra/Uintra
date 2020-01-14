using System;
using System.Collections.Generic;
using UBaseline.Shared.Node;
using Uintra20.Features.Comments.Models;
using Uintra20.Infrastructure.Context;

namespace Uintra20.Features.Comments.Converters.Models
{
    public class CommentsPanelViewModel : NodeViewModel
    {
        public IEnumerable<CommentViewModel> Comments { get; set; }
        public ContextType ActivityId { get; set; }
        public Guid EntityId { get; set; }
        public bool IsReadOnly { get; set; }
    }
}