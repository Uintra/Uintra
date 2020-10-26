using System;
using System.Collections.Generic;
using UBaseline.Shared.Media;
using Uintra.Core.Controls.FileUpload;
using Uintra.Features.Media.Contracts;
using Uintra.Features.Media.Enums;
using Uintra.Features.Media.Models;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra.Features.Media.Helpers
{
    public interface IMediaHelper
    {
        IEnumerable<int> CreateMedia(IContentWithMediaCreateEditModel model, MediaFolderTypeEnum mediaFolderType, Guid? userId = null, int? mediaRootId = null);
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
