using System;
using System.Collections.Generic;
using System.Linq;

namespace Uintra20.Features.Tagging.UserTags.Models
{
    public class TagsPickerViewModel
    {
        public IEnumerable<Guid> TagIdsData { get; set; } = Enumerable.Empty<Guid>();
        public IEnumerable<UserTag> UserTagCollection { get; set; } = Enumerable.Empty<UserTag>();
    }
}