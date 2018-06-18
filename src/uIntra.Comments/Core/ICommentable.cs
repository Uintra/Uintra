using System;
using System.Collections.Generic;

namespace uIntra.Comments
{
    public interface ICommentable
    {
        Guid Id { get; }

        IEnumerable<CommentModel> Comments { get; set; }

        bool IsReadOnly { get; set; }
    }
}
