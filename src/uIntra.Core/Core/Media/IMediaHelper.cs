using System.Collections.Generic;
using uIntra.Core.Controls.FileUpload;
using Umbraco.Core.Models;

namespace uIntra.Core.Media
{
    public interface IMediaHelper
    {
        IEnumerable<int> CreateMedia(IContentWithMediaCreateEditModel model);
        bool DeleteMedia(int mediaId);
        IMedia CreateMedia(TempFile file, int rootMediaId);
        MediaSettings GetMediaFolderSettings(MediaFolderTypeEnum mediaFolderType);
    }
}