using System;
using System.Collections.Generic;

namespace uIntra.Likes
{
    public interface ILikeable
    {
        Guid Id { get; }

        IEnumerable<LikeModel> Likes { get; set; }

        bool IsReadOnly { get; set; }
    }
}
