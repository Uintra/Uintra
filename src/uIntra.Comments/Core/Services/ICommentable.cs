using System;
using System.Collections.Generic;

namespace Uintra.Comments
{
    public interface ICommentable
    {
        Guid Id { get; }

        IEnumerable<CommentModel> Comments { get; set; }

        bool IsReadOnly { get; set; }
    }
}
