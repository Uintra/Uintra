using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using uCommunity.Core.App_Plugins.Core.Caching;
using uCommunity.Core.App_Plugins.Core.Controls.FileUpload.Core;
using uCommunity.Core.App_Plugins.Core.Extentions;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace uCommunity.Core.App_Plugins.Core.Media
{
    public class MediaHelper : IMediaHelper
    {
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly IMediaService _mediaService;

        public MediaHelper(IMemoryCacheService memoryCacheService,
            IMediaService mediaService)
        {
            _memoryCacheService = memoryCacheService;
            _mediaService = mediaService;
        }

        public IEnumerable<int> CreateMedia(IContentWithMediaCreateEditModel model)
        {
            if (model.NewMedia.IsNullOrEmpty()) return Enumerable.Empty<int>();

            var mediaIds = model.NewMedia.Split(';').Where(s => s.IsNotNullOrEmpty()).Select(Guid.Parse);
            var cachedTempMedia = mediaIds.Select(s => _memoryCacheService.Get<TempFile>(s.ToString(), ""));
            var rootMedia = GetRootMedia(model.MediaRootId);

            var umbracoMediaIds = new List<int>();

            foreach (var file in cachedTempMedia)
            {
                var mediaTypeAlias = GetMediaTypeAlias(file.FileBytes);
                var media = _mediaService.CreateMedia(file.FileName, rootMedia, mediaTypeAlias);

                using (var stream = new MemoryStream(file.FileBytes))
                {
                    media.SetValue(UmbracoAliases.Media.UmbracoFilePropertyAlias, Path.GetFileName(file.FileName),
                        stream);
                    stream.Close();
                }
                _mediaService.Save(media);
                umbracoMediaIds.Add(media.Id);
            }
            return umbracoMediaIds;
        }

        private IMedia GetRootMedia(int? rootMediaId)
        {
            if (rootMediaId.HasValue)
            {
                return _mediaService.GetById(rootMediaId.Value);
            }
            return _mediaService.GetRootMedia().First().Parent();
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