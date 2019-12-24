using System;
using System.Collections.Generic;

namespace Uintra20.Features.Bulletins.Models
{
    public class SocialExtendedEditModel : SocialEditModel
    {
        public IEnumerable<Guid> TagIdsData { get; set; }
    }
}