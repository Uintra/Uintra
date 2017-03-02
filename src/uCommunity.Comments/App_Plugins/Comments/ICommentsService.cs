using System;
using System.Collections.Generic;
using uCommunity.Comments.App_Plugins.Comments.Sql;

namespace uCommunity.Comments.App_Plugins.Comments
{
    public interface ICommentsService
    {
        Comment Get(Guid id);

        IEnumerable<Comment> GetMany(Guid activityId);

        int GetCount(Guid activityId);

        bool CanEdit(Comment comment, Guid editorId);

        bool CanDelete(Comment comment, Guid editorId);

        bool WasChanged(Comment comment);

        bool IsReply(Comment comment);

        Comment Create(Guid userId, Guid activityId, string text, Guid? parentId);

        Comment Update(Guid id, string text);

        void Delete(Guid id);
    }
}