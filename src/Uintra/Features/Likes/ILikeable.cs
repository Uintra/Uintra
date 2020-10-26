using System;
using System.Collections.Generic;
using Uintra.Features.Likes.Models;

namespace Uintra.Features.Likes
{
    public interface ILikeable
    {
        Guid Id { get; }
        IEnumerable<LikeModel> Likes { get; set; }
        bool IsReadOnly { get; set; }
    }
}
