using System;
using System.Collections.Generic;
using uIntra.Core.Activity;

namespace uIntra.Likes
{
    public interface ILikeable
    {
        Guid Id { get; }

        IEnumerable<LikeModel> Likes { get; set; }
    }
}
