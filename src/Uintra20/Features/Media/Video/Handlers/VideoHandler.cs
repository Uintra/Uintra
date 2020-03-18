using Compent.CommandBus;
using System.IO;
using UBaseline.Core.Extensions;
using Uintra20.Features.Media.Video.Commands;
using Uintra20.Features.Media.Video.Helpers.Contracts;
using Uintra20.Features.Media.Video.Services.Contracts;
using Uintra20.Infrastructure.Constants;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using LightInject;
using static Uintra20.Infrastructure.Constants.UmbracoAliases.Video;

namespace Uintra20.Features.Media.Video.Handlers
{
    public class VideoHandler : IHandle<VideoConvertedCommand>
    {
        private readonly IVideoHelper _videoHelper;
        private readonly IVideoConverterLogService _videoConverterLogService;
        private readonly IMediaService _mediaService;

        public VideoHandler(
            IVideoHelper videoHelper,
            IVideoConverterLogService videoConverterLogService,
            IMediaService mediaService)
        {
            _videoHelper = videoHelper;
            _videoConverterLogService = videoConverterLogService;
            _mediaService = mediaService;
        }

        public BroadcastResult Handle(VideoConvertedCommand command)
        {
            return Current.Factory.EnsureScope(s =>
            {
                
                var contentTypeBaseServiceProvider = s.GetInstance<IContentTypeBaseServiceProvider>();

                var media = _mediaService.GetById(command.MediaId);

                media.SetValue(ConvertInProcessPropertyAlias, false);

                return command.Success
                    ? OnCreateSuccess(command, media, contentTypeBaseServiceProvider)
                    : OnCreateFail(command, media);
            });
        }

        private BroadcastResult OnCreateSuccess(
            VideoConvertedCommand command,
            IMedia media,
            IContentTypeBaseServiceProvider contentTypeBaseServiceProvider
            )
        {
            using (var fileStream = new FileStream(command.ConvertedFilePath, FileMode.Open, FileAccess.Read))
            using (var memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);
                media.SetValue(contentTypeBaseServiceProvider, UmbracoAliases.Media.UmbracoFilePropertyAlias, media.Name, memoryStream);
            }

            System.IO.File.Delete(command.ConvertedFilePath);
            SaveVideoAdditionProperties(media);
            _mediaService.Save(media);
            _videoConverterLogService.Log(true, "Converted successfully", command.MediaId);

            return BroadcastResult.Success;
        }

        private BroadcastResult OnCreateFail(
            VideoConvertedCommand command,
            IMedia media
            )
        {
            _videoConverterLogService.Log(false, command.Message.ToJson(), command.MediaId);
            media.SetValue(ThumbnailUrlPropertyAlias, _videoHelper.CreateConvertingFailureThumbnail());
            _mediaService.Save(media);

            return BroadcastResult.Failure;
        }

        private void SaveVideoAdditionProperties(IMedia media)
        {
            var thumbnailUrl = _videoHelper.CreateThumbnail(media);
            media.SetValue(ThumbnailUrlPropertyAlias, thumbnailUrl);

            var metadata = _videoHelper.GetSizeMetadata(media);

            media.SetValue(VideoHeightPropertyAlias, metadata.Height);
            media.SetValue(VideoWidthPropertyAlias, metadata.Width);
        }
    }
}
