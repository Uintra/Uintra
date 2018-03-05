using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core;

namespace Uintra.Tagging.UserTags.Models
{
    public class TagPickerViewModel
    {
        public IEnumerable<Guid> TagIdsData { get; set; } = Enumerable.Empty<Guid>();
        public IEnumerable<LabeledIdentity<Guid>> UserTagCollection { get; set; } = Enumerable.Empty<LabeledIdentity<Guid>>();
    }
}