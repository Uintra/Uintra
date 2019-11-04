using System;
using System.Collections.Generic;

namespace Uintra20.Core.Likes
{
    public interface ILikeable
    {
        Guid Id { get; }

        IEnumerable<LikeModel> Likes { get; set; }

        bool IsReadOnly { get; set; }
    }
}
