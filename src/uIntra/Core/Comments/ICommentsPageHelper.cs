using System.Collections.Generic;

namespace uIntra.Core.Comments
{
    public interface ICommentsPageHelper
    {
        IEnumerable<CommentsPageTab> GetCommentsPageTab();
    }
}