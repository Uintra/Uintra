using System;
using System.Collections.Generic;
using uCommunity.Core.Activity;

namespace uCommunity.Likes
{
    public interface ILikeable
    {
        Guid Id { get; }

        IntranetActivityTypeEnum Type { get; }

        IEnumerable<LikeModel> Likes { get; set; }
    }
}
