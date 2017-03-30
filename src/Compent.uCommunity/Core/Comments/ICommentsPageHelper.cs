using System.Collections.Generic;

namespace Compent.uCommunity.Core.Comments
{
    public interface ICommentsPageHelper
    {
        IEnumerable<CommentsPageTab> GetCommentsPageTab();
    }
}