using System;
using System.Collections.Generic;

namespace Uintra20.Features.Social.Models
{
    public class SocialExtendedCreateModel : SocialCreateModel
    {
        public Guid? GroupId { get; set; }
        public IEnumerable<Guid> TagIdsData { get; set; }
    }
}