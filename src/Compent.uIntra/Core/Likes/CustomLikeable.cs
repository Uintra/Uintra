using System;
using System.Collections.Generic;
using uIntra.Likes;

namespace Compent.uIntra.Core.Likes
{
    public class CustomLikeable : ILikeable
    {
        public Guid Id { get; set; }
        
        public IEnumerable<LikeModel> Likes { get; set; }
        public bool IsReadOnly { get; set; }
    }
}