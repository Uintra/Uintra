using System;
using System.Collections.Generic;
using uIntra.Comments;
using uIntra.Core.Activity;

namespace Compent.uIntra.Core.Comments
{
    public class CustomCommentable : ICommentable
    {
        public Guid Id { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}