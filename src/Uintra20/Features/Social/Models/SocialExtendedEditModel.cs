using System;
using System.Collections.Generic;

namespace Uintra20.Features.Social.Models
{
    public class SocialExtendedEditModel : SocialEditModel
    {
        public IEnumerable<Guid> TagIdsData { get; set; }
    }
}