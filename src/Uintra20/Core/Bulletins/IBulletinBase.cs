using System;
using System.Collections.Generic;
using Uintra20.Core.Activity;

namespace Uintra20.Core.Bulletins
{
    public interface IBulletinBase : IIntranetActivity
    {
        int? UmbracoCreatorId { get; set; }
        Guid CreatorId { get; set; }
        IEnumerable<int> MediaIds { get; set; }
        DateTime PublishDate { get; set; }
    }
}
