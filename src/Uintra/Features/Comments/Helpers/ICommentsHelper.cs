using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uintra.Features.Comments.Models;

namespace Uintra.Features.Comments.Helpers
{
    public interface ICommentsHelper
    {
        IEnumerable<CommentViewModel> GetCommentViews(IEnumerable<CommentModel> comments);
        Task<IEnumerable<CommentViewModel>> GetCommentViewsAsync(IEnumerable<CommentModel> comments);
        Task<CommentsOverviewModel> OverViewAsync(Guid activityId);
        Task<CommentsOverviewModel> OverViewAsync(Guid activityId, IEnumerable<CommentModel> comments,bool isReadOnly = false);
    }
}