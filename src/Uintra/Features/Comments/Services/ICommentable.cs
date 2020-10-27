using System;
using System.Collections.Generic;
using Uintra.Features.Comments.Models;

namespace Uintra.Features.Comments.Services
{
    public interface ICommentable
    {
        Guid Id { get; }

        IEnumerable<CommentModel> Comments { get; set; }

        bool IsReadOnly { get; set; }
    }
}
