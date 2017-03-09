using System;
using System.Collections.Generic;
using uCommunity.Core.App_Plugins.Core.Activity;
using uCommunity.Likes.App_Plugins.Likes.Models;

namespace uCommunity.Likes.App_Plugins.Likes
{
    public interface ILikeable
    {
        Guid Id { get; }

        IntranetActivityTypeEnum Type { get; }

        IEnumerable<LikeModel> Likes { get; set; }
    }
}
