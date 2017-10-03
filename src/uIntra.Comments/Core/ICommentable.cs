using System;
using System.Collections.Generic;

namespace uIntra.Comments
{
    public interface ICommentable
    {
        Guid Id { get; }

        IEnumerable<Comment> Comments { get; set; }

        bool IsReadOnly { get; set; }
    }
}
