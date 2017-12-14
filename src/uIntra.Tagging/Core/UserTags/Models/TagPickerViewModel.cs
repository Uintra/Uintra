using System;
using System.Collections.Generic;

namespace uIntra.Tagging.UserTags.Models
{
    public class TagPickerViewModel
    {
        public IEnumerable<Guid> TagIdsData { get; set; }
        public IEnumerable<UserTag> UserTagCollection { get; set; }
    }
}