using System;
using System.Collections.Generic;
using Uintra.Core.Activity;

namespace Uintra.Bulletins
{
    public interface IBulletinBase : IIntranetActivity
    {
        int? UmbracoCreatorId { get; set; }
        Guid CreatorId { get; set; }
        IEnumerable<int> MediaIds { get; set; }
        DateTime PublishDate { get; set; }
    }
}