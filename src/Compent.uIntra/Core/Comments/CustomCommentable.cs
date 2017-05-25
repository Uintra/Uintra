using System;
using System.Collections.Generic;
using uCommunity.Comments;
using uCommunity.Core.Activity;

namespace Compent.uIntra.Core.Comments
{
    public class CustomCommentable : ICommentable
    {
        public Guid Id { get; set; }
        public IntranetActivityTypeEnum Type { get; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}