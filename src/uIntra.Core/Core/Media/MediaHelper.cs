using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using uIntra.Core.Caching;
using uIntra.Core.Controls.FileUpload;
using uIntra.Core.Extentions;
using uIntra.Core.User;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace uIntra.Core.Media
{
    public class MediaHelper : IMediaHelper
    {
        private readonly ICacheService cacheService;
        private readonly IMediaService _mediaService;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        public MediaHelper(ICacheService cacheService,
            IMediaService mediaService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            UmbracoHelper umbracoHelper)
        {
            this.cacheService = cacheService;
            _mediaService = mediaService;
            _intranetUserService = intranetUserService;
            _umbracoHelper = umbracoHelper;
        }

        public IEnumerable<int> CreateMedia(IContentWithMediaCreateEditModel model)
        {
            if (model.NewMedia.IsNullOrEmpty()) return Enumerable.Empty<int>();

            var mediaIds = model.NewMedia.Split(';').Where(s => s.IsNotNullOrEmpty()).Select(Guid.Parse);
            var cachedTempMedia = mediaIds.Select(s => cacheService.Get<TempFile>(s.ToString(), ""));
            var rootMediaId = model.MediaRootId ?? -1;

            var umbracoMediaIds = new List<int>();

            foreach (var file in cachedTempMedia)
            {
                var media = CreateMedia(file, rootMediaId);
                umbracoMediaIds.Add(media.Id);
            }
            return umbracoMediaIds;
        }

        public IMedia CreateMedia(TempFile file, int rootMediaId)
        {
            var mediaTypeAlias = GetMediaTypeAlias(file.FileBytes);
            var media = _mediaService.CreateMedia(file.FileName, rootMediaId, mediaTypeAlias);

            using (var stream = new MemoryStream(file.FileBytes))
            {
                media.SetValue(ImageConstants.IntranetCreatorId, _intranetUserService.GetCurrentUserId().ToString());
                media.SetValue(UmbracoAliases.Media.UmbracoFilePropertyAlias, Path.GetFileName(file.FileName), stream);
                stream.Close();
            }
            _mediaService.Save(media);
            return media;
        }

        public void DeleteMedia(int mediaId)
        {
            var media = _mediaService.GetById(mediaId);
            if (media == null)
            {
                throw new ArgumentNullException($"Media with id = {mediaId} doesn't exist.");
            }

            media.SetValue(ImageConstants.IsDeletedPropertyTypeAlias, true);
            _mediaService.Save(media);
        }

        public void DeleteMedia(IEnumerable<int> mediaIds)
        {
            foreach (var id in mediaIds)
            {
                DeleteMedia(id);
            }
        }

        public MediaSettings GetMediaFolderSettings(MediaFolderTypeEnum mediaFolderType)
        {
            var folders = _umbracoHelper.TypedMediaAtRoot().Where(m => m.DocumentTypeAlias.Equals(UmbracoAliases.Media.FolderTypeAlias));
            var mediaFolder = folders.Single(m => m.GetPropertyValue<MediaFolderTypeEnum>(FolderConstants.FolderTypePropertyTypeAlias) == mediaFolderType);

            return new MediaSettings
            {
                AllowedMediaExtentions = mediaFolder.GetPropertyValue<string>(FolderConstants.AllowedMediaExtensionsPropertyTypeAlias, String.Empty),
                MediaRootId = mediaFolder.Id
            };
        }

        public bool IsMediaDeleted(IPublishedContent media)
        {
            return media.HasProperty(ImageConstants.IsDeletedPropertyTypeAlias) && media.GetPropertyValue<bool>(ImageConstants.IsDeletedPropertyTypeAlias, false);
        }

        private string GetMediaTypeAlias(byte[] fileBytes)
        {
            return IsFileImage(fileBytes) ? UmbracoAliases.Media.ImageTypeAlias : UmbracoAliases.Media.FileTypeAlias;
        }

        private bool IsFileImage(byte[] fileBytes)
        {
            bool fileIsImage;
            try
            {
                using (var stream = new MemoryStream(fileBytes))
                {
                    Image.FromStream(stream).Dispose();
                }
                fileIsImage = true;
            }
            catch
            {
                fileIsImage = false;
            }

            return fileIsImage;
        }
    }
}