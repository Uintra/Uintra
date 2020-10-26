using System;
using System.Collections.Generic;
using Uintra.Core.Activity.Entities;

namespace Uintra.Features.Social
{
    public interface ISocialBase : IIntranetActivity
    {
        int? UmbracoCreatorId { get; set; }
        Guid CreatorId { get; set; }
        IEnumerable<int> MediaIds { get; set; }
        DateTime PublishDate { get; set; }
    }
}
