using Compent.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using UBaseline.Core.Media;
using Uintra20.Core.Controls.FileUpload;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Media.Constants;
using Uintra20.Features.Media.Contracts;
using Uintra20.Features.Media.Enums;
using Uintra20.Features.Media.Images.Helpers.Contracts;
using Uintra20.Features.Media.Models;
using Uintra20.Features.Media.Video.Converters.Contracts;
using Uintra20.Features.Media.Video.Helpers.Contracts;
using Uintra20.Infrastructure.Caching;
using Uintra20.Infrastructure.Constants;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.TypeProviders;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using static Uintra20.Infrastructure.Constants.UmbracoAliases.Media;
using FolderModel = Uintra20.Core.UbaselineModels.FolderModel;

namespace Uintra20.Features.Media.Helpers
{
    public class MediaHelper : IMediaHelper

    {
        private readonly ICacheService _cacheService;
        private readonly IMediaModelService _mediaModelService;
        private readonly IMediaService _mediaService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IMediaFolderTypeProvider _mediaFolderTypeProvider;
        private readonly IImageHelper _imageHelper;
        private readonly IVideoHelper _videoHelper;
        private readonly IVideoConverter _videoConverter;

        public MediaHelper(ICacheService cacheService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IMediaModelService mediaModelService,
            IMediaService mediaService,
            IMediaFolderTypeProvider mediaFolderTypeProvider,
            IImageHelper imageHelper,
            IVideoHelper videoHelper,
            IVideoConverter videoConverter)
        {
            _cacheService = cacheService;
            _intranetMemberService = intranetMemberService;
            _mediaModelService = mediaModelService;
            _mediaService = mediaService;
            _mediaFolderTypeProvider = mediaFolderTypeProvider;
            _imageHelper = imageHelper;
            _videoHelper = videoHelper;
            _videoConverter = videoConverter;
        }
        public IEnumerable<int> CreateMedia(
            IContentWithMediaCreateEditModel model,
            MediaFolderTypeEnum mediaFolderType,
            Guid? userId = null,
            int? mediaRootId = null)
        {
            if (model.NewMedia.IsNullOrEmpty()) return Enumerable.Empty<int>();

            var mediaIds = model.NewMedia.Split(',').Where(s => s.HasValue()).Select(Guid.Parse).ToList();
            var cachedTempMedia = mediaIds.Select(s => _cacheService.Get<TempFile>(s.ToString(), string.Empty));

            int rootMediaId;

            if (mediaRootId.HasValue)
            {
                rootMediaId = mediaRootId.Value;
            }
            else
            {
                var settings = GetMediaFolderSettings(mediaFolderType, createFolderIfNotExists: true);
                rootMediaId = settings.MediaRootId ?? -1;
            }

            var createdMediaIds = cachedTempMedia.Select(file =>
                 {
                     var media = CreateMedia(file, rootMediaId, userId);
                     return media.Id;
                 })
                .ToList();

            return createdMediaIds;
        }

        public IMedia CreateMedia(TempFile file, int rootMediaId, Guid? userId = null)
        {
            userId = userId ?? _intranetMemberService.GetCurrentMemberId();

            var mediaTypeAlias = GetMediaTypeAlias(file);
            var media = _mediaService.CreateMedia(file.FileName, rootMediaId, mediaTypeAlias);
            if (_videoConverter.NeedConvert(mediaTypeAlias, file.FileName))
            {
                media.SetValue(IntranetConstants.IntranetCreatorId, userId.ToString());
                media.SetValue(UmbracoAliases.Video.ThumbnailUrlPropertyAlias, _videoHelper.CreateConvertingThumbnail());
                media.SetValue(UmbracoAliases.Video.ConvertInProcessPropertyAlias, true);
                _mediaService.Save(media);

                Task.Run(() =>
                {
                    _videoConverter.Convert(new MediaConvertModel
                    {
                        File = file,
                        MediaId = media.Id
                    });
                });

                return media;
            }

            var stream = new MemoryStream(file.FileBytes);
            if (_imageHelper.IsFileImage(file.FileBytes))
            {
                var fileStream = new MemoryStream(file.FileBytes, 0, file.FileBytes.Length, true, true);
                stream = _imageHelper.NormalizeOrientation(fileStream, Path.GetExtension(file.FileName));
                fileStream.Close();
            }

            media.SetValue(IntranetConstants.IntranetCreatorId, userId.ToString());
            media.SetValue(Current.Services.ContentTypeBaseServices, "umbracoFile", Path.GetFileName(file.FileName), stream);
            stream.Close();

            if (mediaTypeAlias == VideoTypeAlias)
            {
                SaveVideoAdditionProperties(media);
            }
            _mediaService.Save(media);
            return media;
        }
        /// <summary>
        /// TODO There is an ubaseline implementation of creation of media
        /// (key difference see at cs. line 185, 165) 
        /// </summary>
        /// <param name="mediaId"></param>
        //public IMediaModel CreateMedia(TempFile file, int rootMediaId, Guid? userId = null)
        //{
        //    userId = userId ?? _intranetMemberService.GetCurrentMemberId();
        //    var mediaTypeAlias = GetMediaTypeAlias(file);
        //    var media = _mediaService.CreateMedia(file.FileName, rootMediaId, mediaTypeAlias);
        //    if (_videoConverter.NeedConvert(mediaTypeAlias, file.FileName))
        //    {
        //        media.SetValue(IntranetConstants.IntranetCreatorId, userId.ToString());
        //        media.SetValue(UmbracoAliases.Video.ThumbnailUrlPropertyAlias, _videoHelper.CreateConvertingThumbnail());
        //        media.SetValue(UmbracoAliases.Video.ConvertInProcessPropertyAlias, true);
        //        _mediaService.Save(media);
        //        Task.Run(() =>
        //        {
        //            _videoConverter.Convert(new MediaConvertModel
        //            {
        //                File = file,
        //                MediaId = media.Id
        //            });
        //        });
        //        return _mediaModelService.Get(media.Id);
        //    }
        //    var stream = new MemoryStream(file.FileBytes);
        //    if (_imageHelper.IsFileImage(file.FileBytes))
        //    {
        //        var fileStream = new MemoryStream(file.FileBytes, 0, file.FileBytes.Length, true, true);
        //        stream = _imageHelper.NormalizeOrientation(fileStream, Path.GetExtension(file.FileName));
        //        fileStream.Close();
        //    }
        //    media.SetValue(IntranetConstants.IntranetCreatorId, userId.ToString());
        //    media.SetValue(Current.Services.ContentTypeBaseServices, "umbracoFile", Path.GetFileName(file.FileName), stream);
        //    stream.Close();
        //    if (mediaTypeAlias == VideoTypeAlias)
        //    {
        //        SaveVideoAdditionProperties(media);
        //    }
        //    _mediaService.Save(media);
        //    return _mediaModelService.Get(media.Id);
        //}
        public void DeleteMedia(int mediaId)
        {
            var media = _mediaService.GetById(mediaId);
            if (media == null)
            {
                throw new ArgumentNullException($"Media with id = {mediaId} doesn't exist.");
            }

            media.SetValue(IsDeletedPropertyTypeAlias, true);
            _mediaService.Save(media);
        }

        public void DeleteMedia(string mediaPath)
        {
            var media = _mediaService.GetMediaByPath(mediaPath);
            if (media == null)
            {
                throw new ArgumentNullException($"Media \"{mediaPath}\" doesn't exist.");
            }

            media.SetValue(IsDeletedPropertyTypeAlias, true);
            _mediaService.Save(media);
        }

        public void DeleteMedia(IEnumerable<int> mediaIds)
        {
            var medias = _mediaService.GetByIds(mediaIds).ToList();

            foreach (var media in medias)
            {
                media.SetValue(IsDeletedPropertyTypeAlias, true);
            }

            _mediaService.Save(medias);
        }

        public void RestoreMedia(int mediaId)
        {
            var media = _mediaService.GetById(mediaId);
            if (media == null)
            {
                throw new ArgumentNullException($"Media with id = {mediaId} doesn't exist.");
            }

            media.SetValue(IsDeletedPropertyTypeAlias, false);
            _mediaService.Save(media);
        }

        public void RestoreMedia(IEnumerable<int> mediaIds)
        {
            var medias = _mediaService.GetByIds(mediaIds).ToList();

            foreach (var media in medias)
            {
                media.SetValue(IsDeletedPropertyTypeAlias, false);
            }

            _mediaService.Save(medias);
        }

        public MediaSettings GetMediaFolderSettings(int mediaFolderType, bool createFolderIfNotExists = false)
        {
            var folderType = _mediaFolderTypeProvider.All.Get(mediaFolderType);

            var result = GetMediaFolderSettings(folderType, createFolderIfNotExists);

            return result;
        }

        public MediaSettings GetMediaFolderSettings(Enum mediaFolderType, bool createFolderIfNotExists = false)
        {
            var mediaFolder = GetMediaFolder(mediaFolderType);
            if (mediaFolder == null)
            {
                if (createFolderIfNotExists)
                {
                    mediaFolder = CreateMediaFolder(mediaFolderType);
                }
                else
                {
                    return null;
                }
            }

            return new MediaSettings
            {
                AllowedMediaExtensions = GetAllowedMediaExtensions(mediaFolder),
                MediaRootId = mediaFolder.Id
            };
        }

        //public bool IsMediaDeleted(IPublishedContent media)
        //{
        //    return media.HasProperty(IsDeletedPropertyTypeAlias) && media.Value<bool>(IsDeletedPropertyTypeAlias);
        //}

        public static IEnumerable<string> GetMediaUrls(IEnumerable<int> ids)
        {
            if (!ids.Any()) return Enumerable.Empty<string>();

            var mediaProvider = DependencyResolver.Current.GetService<IMediaProvider>();

            return ids.Select(id => mediaProvider.GetById(id)?.Url).Where(url => url.HasValue());
        }

        private void SaveVideoAdditionProperties(IMedia media)
        {
            var thumbnailUrl = _videoHelper.CreateThumbnail(media);
            media.SetValue(UmbracoAliases.Video.ThumbnailUrlPropertyAlias, thumbnailUrl);

            var videoSizeMetadata = _videoHelper.GetSizeMetadata(media);
            media.SetValue(UmbracoAliases.Video.VideoHeightPropertyAlias, videoSizeMetadata.Height);
            media.SetValue(UmbracoAliases.Video.VideoWidthPropertyAlias, videoSizeMetadata.Width);
        }

        private string GetMediaTypeAlias(TempFile file)
        {
            var extension = Path.GetExtension(file.FileName);

            var isVideo = _videoHelper.IsVideo(extension);

            if (isVideo) return VideoTypeAlias;

            return _imageHelper.IsFileImage(file.FileBytes)
                ? ImageTypeAlias
                : FileTypeAlias;
        }

        private string GetAllowedMediaExtensions(FolderModel mediaFolderContent)
        {
            var allowedMediaExtensions = mediaFolderContent.AllowedMediaExtensions ?? string.Empty;

            var result = allowedMediaExtensions
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(ext =>
                {
                    var trimmedExt = ext.Trim().ToLower();
                    return trimmedExt.StartsWith(".") ? trimmedExt : $".{trimmedExt}";
                });

            return result.JoinWith();
        }

        private FolderModel GetMediaFolder(Enum mediaFolderType)
        {
            var folders = _mediaModelService.AsEnumerable().OfType<FolderModel>().Where(x => x.ParentId == -1);

            var mediaFolder = folders.SingleOrDefault(f =>
            {
                var folderType = f.FolderType.Value;
                return folderType.HasValue() && folderType.Equals(mediaFolderType.ToString());
            });

            return mediaFolder;
        }

        private FolderModel CreateMediaFolder(Enum mediaFolderType)
        {
            var mediaFolderTypeEnum = (MediaFolderTypeEnum)mediaFolderType;
            var folderName = mediaFolderTypeEnum.GetAttribute<DisplayAttribute>().Name;
            var mediaFolder = _mediaService.CreateMedia(folderName, -1, FolderTypeAlias);
            mediaFolder.SetValue(FolderConstants.FolderTypePropertyTypeAlias, mediaFolderType.ToString());
            _mediaService.Save(mediaFolder);

            return _mediaModelService.Get<FolderModel>(mediaFolder.Id);
        }
    }
}
