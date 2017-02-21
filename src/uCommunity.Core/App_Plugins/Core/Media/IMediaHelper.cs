using System.Collections.Generic;

namespace uCommunity.Core.App_Plugins.Core.Media
{
    public interface IMediaHelper
    {
        IEnumerable<int> CreateMedia(IContentWithMediaCreateEditModel model);
    }
}