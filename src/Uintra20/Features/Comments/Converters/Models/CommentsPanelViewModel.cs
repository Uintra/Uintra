using System;
using System.Collections.Generic;
using UBaseline.Shared.Node;
using Uintra20.Features.Comments.Models;

namespace Uintra20.Features.Comments.Converters.Models
{
    public class CommentsPanelViewModel : NodeViewModel
    {
        public IEnumerable<CommentViewModel> Comments { get; set; }

        public Guid ActivityId { get; set; }

        public string ElementId { get; set; }

        public bool IsReadOnly { get; set; }
    }
}