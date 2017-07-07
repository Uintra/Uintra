using System.Collections.Generic;

namespace Compent.uIntra.Core.Comments
{
    public interface ICommentsPageHelper
    {
        IEnumerable<CommentsPageTab> GetCommentsPageTab();
    }
}