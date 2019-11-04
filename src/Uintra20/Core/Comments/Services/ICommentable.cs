using System;
using System.Collections.Generic;

namespace Uintra20.Core.Comments
{
    public interface ICommentable
    {
        Guid Id { get; }

        IEnumerable<CommentModel> Comments { get; set; }

        bool IsReadOnly { get; set; }
    }
}
