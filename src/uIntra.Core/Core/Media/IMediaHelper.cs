using System;
using System.Collections.Generic;
using Uintra.Core.Controls.FileUpload;
using Umbraco.Core.Models;

namespace Uintra.Core.Media
{
    public interface IMediaHelper
    {
        IEnumerable<int> CreateMedia(IContentWithMediaCreateEditModel model, Guid? userId = null);
        void DeleteMedia(int mediaId);
        void DeleteMedia(string mediaPath);
        void DeleteMedia(IEnumerable<int> mediaIds);
        void RestoreMedia(int mediaId);
        void RestoreMedia(IEnumerable<int> mediaIds);
        IMedia CreateMedia(TempFile file, int rootMediaId, Guid? userId = null);
        bool IsMediaDeleted(IPublishedContent media);

        MediaSettings GetMediaFolderSettings(Enum mediaFolderType, bool createFolderIfNotExists = false);
    }
}