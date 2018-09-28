using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Compent.CommandBus;
using Compent.Extensions;
using Uintra.Core.Caching;
using Uintra.Core.Constants;
using Uintra.Core.Controls.FileUpload;
using Uintra.Core.Extensions;
using Uintra.Core.TypeProviders;
using Uintra.Core.User;
using UIntra.Core.Media;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using static Uintra.Core.Constants.UmbracoAliases.Media;
using File = System.IO.File;
using Task = System.Threading.Tasks.Task;

namespace Uintra.Core.Media
{
    public class MediaHelper : IMediaHelper, IHandle<VideoConvertedCommand>
    {
        private readonly ICacheService _cacheService;
        private readonly IMediaService _mediaService;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IMediaFolderTypeProvider _mediaFolderTypeProvider;
        private readonly IImageHelper _imageHelper;
        private readonly IVideoHelper _videoHelper;
        private readonly IVideoConverter _videoConverter;
        private readonly IVideoConverterLogService _videoConverterLogService;

        public MediaHelper(ICacheService cacheService,
            IMediaService mediaService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            UmbracoHelper umbracoHelper,
            IMediaFolderTypeProvider mediaFolderTypeProvider,
            IImageHelper imageHelper,
            IVideoHelper videoHelper,
            IVideoConverter videoConverter,
            IVideoConverterLogService videoConverterLogService)
        {
            _cacheService = cacheService;
            _mediaService = mediaService;
            _intranetUserService = intranetUserService;
            _umbracoHelper = umbracoHelper;
            _mediaFolderTypeProvider = mediaFolderTypeProvider;
            _imageHelper = imageHelper;
            _videoHelper = videoHelper;
            _videoConverter = videoConverter;
            _videoConverterLogService = videoConverterLogService;
        }

        public IEnumerable<int> CreateMedia(IContentWithMediaCreateEditModel model, Guid? userId = null)
        {
            if (model.NewMedia.IsNullOrEmpty()) return Enumerable.Empty<int>();

            var mediaIds = model.NewMedia.Split(';').Where(s => s.HasValue()).Select(Guid.Parse);
            var cachedTempMedia = mediaIds.Select(s => _cacheService.Get<TempFile>(s.ToString(), ""));
            var rootMediaId = model.MediaRootId ?? -1;

            var umbracoMediaIds = new List<int>();

            foreach (var file in cachedTempMedia)
            {
                var media = CreateMedia(file, rootMediaId, userId);
                umbracoMediaIds.Add(media.Id);
            }
            return umbracoMediaIds;
        }

        public IMedia CreateMedia(TempFile file, int rootMediaId, Guid? userId = null)
        {
            userId = userId ?? _intranetUserService.GetCurrentUserId();

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
                    _videoConverter.Convert(new MediaConvertModel()
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
            }

            media.SetValue(IntranetConstants.IntranetCreatorId, userId.ToString());
            media.SetValue(UmbracoFilePropertyAlias, Path.GetFileName(file.FileName), stream);
            stream.Close();

            if (mediaTypeAlias == VideoTypeAlias)
            {
                SaveVideoAdditionProperties(media);
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

        public bool IsMediaDeleted(IPublishedContent media)
        {
            return media.HasProperty(IsDeletedPropertyTypeAlias) && media.GetPropertyValue<bool>(IsDeletedPropertyTypeAlias, false);
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
            if (_videoHelper.IsVideo(Path.GetExtension(file.FileName)?.ToLower())) return VideoTypeAlias;

            return _imageHelper.IsFileImage(file.FileBytes) ? ImageTypeAlias : FileTypeAlias;
        }

        private string GetAllowedMediaExtensions(IPublishedContent mediaFolderContent)
        {
            var allowedMediaExtensions = mediaFolderContent.GetPropertyValue<string>(FolderConstants.AllowedMediaExtensionsPropertyTypeAlias, string.Empty);

            var result = allowedMediaExtensions
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(ext =>
                {
                    var trimmedExt = ext.Trim().ToLower();
                    return trimmedExt.StartsWith(".") ? trimmedExt : $".{trimmedExt}";
                });

            return result.JoinWithComma();
        }

        private IPublishedContent GetMediaFolder(Enum mediaFolderType)
        {
            var folders = _umbracoHelper.TypedMediaAtRoot().Where(m => m.DocumentTypeAlias.Equals(FolderTypeAlias));

            var mediaFolder = folders.SingleOrDefault(f =>
            {
                var folderType = f.GetPropertyValue<string>(FolderConstants.FolderTypePropertyTypeAlias);
                return folderType.HasValue() && folderType.Equals(mediaFolderType.ToString());
            });

            return mediaFolder;
        }

        private IPublishedContent CreateMediaFolder(Enum mediaFolderType)
        {
            var mediaFolderTypeEnum = (MediaFolderTypeEnum)mediaFolderType;
            var folderName = mediaFolderTypeEnum.GetAttribute<DisplayAttribute>().Name;
            var mediaFolder = _mediaService.CreateMedia(folderName, -1, FolderTypeAlias);
            mediaFolder.SetValue(FolderConstants.FolderTypePropertyTypeAlias, mediaFolderType.ToString());
            _mediaService.Save(mediaFolder);

            return _umbracoHelper.TypedMedia(mediaFolder.Id);
        }

        public BroadcastResult Handle(VideoConvertedCommand command)
        {
            var media = _mediaService.GetById(command.MediaId);
            media.SetValue(UmbracoAliases.Video.ConvertInProcessPropertyAlias, false);

            if (!command.Success)
            {
                _videoConverterLogService.Log(false, command.Message.ToJson(), command.MediaId);

                media.SetValue(UmbracoAliases.Video.ThumbnailUrlPropertyAlias, _videoHelper.CreateConvertingFailureThumbnail());                
                _mediaService.Save(media);

                return BroadcastResult.Failure;
            }

            using (var fs = new FileStream(command.ConvertedFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var ms = new MemoryStream())
                {
                    fs.CopyTo(ms);
                    media.SetValue(UmbracoFilePropertyAlias, Path.GetFileName(command.ConvertedFilePath), ms);
                }
            }

            File.Delete(command.ConvertedFilePath);

            SaveVideoAdditionProperties(media);

            _mediaService.Save(media);

            _videoConverterLogService.Log(true, "Converted succesfully", command.MediaId);

            return BroadcastResult.Success;
        }
    }
}