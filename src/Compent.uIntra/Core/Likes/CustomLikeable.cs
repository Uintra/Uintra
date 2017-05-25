using System;
using System.Collections.Generic;
using uCommunity.Core.Activity;
using uCommunity.Likes;

namespace Compent.uCommunity.Core.Likes
{
    public class CustomLikeable : ILikeable
    {
        public Guid Id { get; set; }

        public IntranetActivityTypeEnum Type { get; set; }

        public IEnumerable<LikeModel> Likes { get; set; }
    }
}