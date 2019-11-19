using System;
using System.Collections.Generic;
using Uintra20.Features.Comments.Models;

namespace Uintra20.Features.Comments.Services
{
    public interface ICommentable
    {
        Guid Id { get; }

        IEnumerable<CommentModel> Comments { get; set; }

        bool IsReadOnly { get; set; }
    }
}
