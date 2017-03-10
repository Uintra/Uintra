using System;
using System.Collections.Generic;
using uCommunity.Core.App_Plugins.Core.Activity;

namespace uCommunity.Likes
{
    public interface ILikeable
    {
        Guid Id { get; }

        IntranetActivityTypeEnum Type { get; }

        IEnumerable<LikeModel> Likes { get; set; }
    }
}
