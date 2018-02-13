using System;
using System.Collections.Generic;
using System.Linq;

namespace Uintra.Groups
{
    public class GroupDocumentsTableViewModel
    {
        public Guid GroupId { get; set; }
        public GroupDocumentDocumentField Column { get; set; }
        public Direction Direction { get; set; }
        public IEnumerable<GroupDocumentTableRowViewModel> Documents { get; set; } = Enumerable.Empty<GroupDocumentTableRowViewModel>();
    }
}