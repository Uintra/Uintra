using System.Collections.Generic;
using uIntra.Core.Controls.FileUpload;
using uIntra.Core.TypeProviders;
using Umbraco.Core.Models;

namespace uIntra.Core.Media
{
    public interface IMediaHelper
    {
        IEnumerable<int> CreateMedia(IContentWithMediaCreateEditModel model);
        void DeleteMedia(int mediaId);
        void DeleteMedia(IEnumerable<int> mediaIds);
        void RestoreMedia(int mediaId);
        void RestoreMedia(IEnumerable<int> mediaIds);
        IMedia CreateMedia(TempFile file, int rootMediaId);
        bool IsMediaDeleted(IPublishedContent media);

        MediaSettings GetMediaFolderSettings(int mediaFolderType);
        MediaSettings GetMediaFolderSettings(IIntranetType mediaFolderType);
    }
}