using System;
using System.Collections.Generic;
using Uintra20.Features.Activity.Entities;

namespace Uintra20.Features.Bulletins
{
    public interface IBulletinBase : IIntranetActivity
    {
        int? UmbracoCreatorId { get; set; }
        Guid CreatorId { get; set; }
        IEnumerable<int> MediaIds { get; set; }
        DateTime PublishDate { get; set; }
    }
}
