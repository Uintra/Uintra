using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Activity.Models;
using Uintra.Core.Feed.Settings;

namespace Uintra.Core.Feed.Models
{
    public class FeedListViewModel
    {
        public Enum Type { get; set; }
        public IEnumerable<IntranetActivityPreviewModelBase> Feed { get; set; } = Enumerable.Empty<IntranetActivityPreviewModelBase>();
        public FeedTabSettings TabSettings { get; set; }
        public bool BlockScrolling { get; set; }
        public bool IsReadOnly { get; set; }
    }
}