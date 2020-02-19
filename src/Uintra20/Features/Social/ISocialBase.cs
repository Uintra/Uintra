using System;
using System.Collections.Generic;
using Uintra20.Core.Activity.Entities;

namespace Uintra20.Features.Social
{
    public interface ISocialBase : IIntranetActivity
    {
        int? UmbracoCreatorId { get; set; }
        Guid CreatorId { get; set; }
        IEnumerable<int> MediaIds { get; set; }
        DateTime PublishDate { get; set; }
    }
}
