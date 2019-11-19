using System;
using System.Collections.Generic;
using Uintra20.Features.Likes.Models;

namespace Uintra20.Features.Likes
{
    public interface ILikeable
    {
        Guid Id { get; }

        IEnumerable<LikeModel> Likes { get; set; }

        bool IsReadOnly { get; set; }
    }
}
