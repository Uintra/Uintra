using System;
using System.Collections.Generic;
using uIntra.Core.Activity;
using uIntra.Core.User;

namespace uIntra.Bulletins
{
    public interface IBulletinBase : IIntranetActivity
    {
        int? UmbracoCreatorId { get; set; }
        Guid CreatorId { get; set; }
        IIntranetUser Creator { get; set; }
        IEnumerable<int> MediaIds { get; set; }
        DateTime PublishDate { get; set; }
    }
}