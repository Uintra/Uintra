using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core;

namespace uIntra.Tagging.UserTags.Models
{
    public class TagPickerViewModel
    {
        public IEnumerable<Guid> TagIdsData { get; set; } = Enumerable.Empty<Guid>();
        public IEnumerable<LabeledIdentity<Guid>> UserTagCollection { get; set; } = Enumerable.Empty<LabeledIdentity<Guid>>();
    }
}