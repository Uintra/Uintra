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

        public VideoHandler(
            IVideoHelper videoHelper,
            IVideoConverterLogService videoConverterLogService)
        {
            _videoHelper = videoHelper;
            _videoConverterLogService = videoConverterLogService;
        }

        public BroadcastResult Handle(VideoConvertedCommand command)
        {
            return Current.Factory.EnsureScope(s =>
            {
                
                var contentTypeBaseServiceProvider = s.GetInstance<IContentTypeBaseServiceProvider>();

                var mediaService = s.GetInstance<IMediaService>();

                var media = mediaService.GetById(command.MediaId);

                media.SetValue(ConvertInProcessPropertyAlias, false);

                return command.Success
                    ? OnCreateSuccess(command, media, contentTypeBaseServiceProvider, mediaService)
                    : OnCreateFail(command, media, mediaService);
            });
        }

        private BroadcastResult OnCreateSuccess(
            VideoConvertedCommand command,
            IMedia media,
            IContentTypeBaseServiceProvider contentTypeBaseServiceProvider,
            IMediaService mediaService
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
            mediaService.Save(media);
            _videoConverterLogService.Log(true, "Converted successfully", command.MediaId);

            return BroadcastResult.Success;
        }

        private BroadcastResult OnCreateFail(
            VideoConvertedCommand command,
            IMedia media,
            IMediaService mediaService
            )
        {
            _videoConverterLogService.Log(false, command.Message.ToJson(), command.MediaId);
            media.SetValue(ThumbnailUrlPropertyAlias, _videoHelper.CreateConvertingFailureThumbnail());
            mediaService.Save(media);

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
