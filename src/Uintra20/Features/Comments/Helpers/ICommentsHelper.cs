using System;
using System.Collections.Generic;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Features.Comments.Models;
using Uintra20.Features.Likes.Models;

namespace Uintra20.Features.Comments.Helpers
{
    public interface ICommentsHelper
    {
        IEnumerable<CommentViewModel> GetCommentViews(IEnumerable<CommentModel> comments);
        CommentViewModel GetCommentView(CommentModel comment, Guid currentMemberId, IIntranetMember creator);
        LikesViewModel GetLikesViewModel(Guid commentId);
    }
}