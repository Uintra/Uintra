﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Uintra20.Core.Controls.FileUpload;
using Uintra20.Core.UmbracoEvents.Services.Contracts;
using Uintra20.Features.Media;
using Uintra20.Infrastructure.Constants;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra20.Core.UmbracoEvents.Services.Implementations
{
    public class VideoConvertEventService : IUmbracoMediaSavedEventService
    {
        private readonly IVideoConverter _videoConverter;
        private readonly IVideoHelper _videoHelper;
        private readonly IContentTypeService _contentTypeService;
        private readonly IMediaService _mediaService;
        private readonly IMediaTypeService _mediaTypeService;


        public VideoConvertEventService(
            IVideoConverter videoConverter,
            IVideoHelper videoHelper,
            IContentTypeService contentTypeService,
            IMediaService mediaService,
            IMediaTypeService mediaTypeService)
        {
            _videoConverter = videoConverter;
            _videoHelper = videoHelper;
            _contentTypeService = contentTypeService;
            _mediaService = mediaService;
            _mediaTypeService = mediaTypeService;
        }

        public void ProcessMediaSaved(IMediaService sender, SaveEventArgs<IMedia> args)
        {
            var newMedia = args
                .SavedEntities
                .Where(m => m.WasPropertyDirty("Id")).ToList();

            foreach (var media in newMedia)
            {
                var convertInProgress = _videoConverter.IsConverting(media);
                if (convertInProgress || media.ContentType.Alias == UmbracoAliases.Media.ImageTypeAlias)
                {
                    continue;
                }
                
                var umbracoFile = media.GetValue<string>(UmbracoAliases.Media.UmbracoFilePropertyAlias);

                if (_videoConverter.IsVideo(umbracoFile))
                {
                    var videoContentType = _mediaTypeService.Get(UmbracoAliases.Media.VideoTypeAlias); // BECOME
                    
                    //media.ChangeContentType(videoContentType, false);// TODO Resolve changeContentType umbraco v8

                    // Due to ChangeContentType is internal
                    var method = typeof(Media).GetMethod("ChangeContentType", BindingFlags.NonPublic | BindingFlags.Instance);

                    method?.Invoke(this, new object[]
                    {
                        videoContentType,
                        false
                    });
                    
                    _mediaService.Save(media);

                    var video = _mediaService.GetById(media.Id);

                    if (_videoConverter.IsMp4(umbracoFile))
                    {
                        var thumbnailUrl = _videoHelper.CreateThumbnail(video);
                        video.SetValue(UmbracoAliases.Video.ThumbnailUrlPropertyAlias, thumbnailUrl);
                        _mediaService.Save(video);

                        continue;
                    }

                    video.SetValue(UmbracoAliases.Video.ThumbnailUrlPropertyAlias, _videoHelper.CreateConvertingThumbnail());
                    _mediaService.Save(video);

                    var fileBytes = System.IO.File.ReadAllBytes(Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + umbracoFile));

                    System.Threading.Tasks.Task.Run(() =>
                    {
                        _videoConverter.Convert(new MediaConvertModel()
                        {
                            File = new TempFile { FileName = umbracoFile, FileBytes = fileBytes },
                            MediaId = media.Id
                        });
                    });
                }
            }
        }
    }
}