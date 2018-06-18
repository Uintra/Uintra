using System.Collections.Generic;

namespace Compent.Uintra.Core.Comments
{
    public interface ICommentsPageHelper
    {
        IEnumerable<CommentsPageTab> GetCommentsPageTab();
    }
}