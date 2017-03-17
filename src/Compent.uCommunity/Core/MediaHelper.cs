using System.Collections.Generic;
using System.Linq;
using uCommunity.Core.Media;

namespace Compent.uCommunity.Core
{
    public class MediaHelper : IMediaHelper
    {
        public IEnumerable<int> CreateMedia(IContentWithMediaCreateEditModel model)
        {
            return Enumerable.Empty<int>();
        }
    }
}