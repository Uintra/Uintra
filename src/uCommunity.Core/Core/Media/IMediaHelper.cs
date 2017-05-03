using System.Collections.Generic;
using uCommunity.Core.Controls.FileUpload;
using Umbraco.Core.Models;

namespace uCommunity.Core.Media
{
    public interface IMediaHelper
    {
        IEnumerable<int> CreateMedia(IContentWithMediaCreateEditModel model);
        IMedia CreateMedia(TempFile file, IMedia rootMedia);
    }
}