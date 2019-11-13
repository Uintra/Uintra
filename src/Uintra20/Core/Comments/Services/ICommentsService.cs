using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Uintra20.Core.Comments
{
    public interface ICommentsService
    {
        Task<CommentModel> GetAsync(Guid id);

        Task<IEnumerable<CommentModel>> GetManyAsync(Guid activityId);

        Task<int> GetCountAsync(Guid activityId);

        bool CanEdit(CommentModel comment, Guid editorId);

        bool CanDelete(CommentModel comment, Guid editorId);

        bool WasChanged(CommentModel comment);

        bool IsReply(CommentModel comment);

        Task<CommentModel> CreateAsync(CommentCreateDto dto);

        Task<CommentModel> UpdateAsync(CommentEditDto dto);

        Task DeleteAsync(Guid id);

        Task FillCommentsAsync(ICommentable entity);

        string GetCommentViewId(Guid commentId);

        Task<bool> IsExistsUserCommentAsync(Guid activityId, Guid userId);


        CommentModel Get(Guid id);

        IEnumerable<CommentModel> GetMany(Guid activityId);

        int GetCount(Guid activityId);

        CommentModel Create(CommentCreateDto dto);

        CommentModel Update(CommentEditDto dto);

        void Delete(Guid id);

        void FillComments(ICommentable entity);

        bool IsExistsUserComment(Guid activityId, Guid userId);
    }
}
