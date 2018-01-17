using System;
using System.Collections.Generic;
using uIntra.Comments;

namespace Compent.uIntra.Core.Comments
{
    public class CustomCommentable : ICommentable
    {
        public Guid Id { get; set; }
        public IEnumerable<CommentModel> Comments { get; set; }
        public bool IsReadOnly { get; set; }
    }
}