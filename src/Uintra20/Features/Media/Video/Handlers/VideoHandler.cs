using Compent.CommandBus;
using System.IO;
using UBaseline.Core.Extensions;
using Uintra20.Features.Media.Video.Commands;
using Uintra20.Features.Media.Video.Helpers.Contracts;
using Uintra20.Features.Media.Video.Services.Contracts;
using Uintra20.Infrastructure.Constants;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra20.Features.Media.Video.Handlers
{
    public class VideoHandler : IHandle<VideoConvertedCommand>
    {
        private readonly IMediaService _mediaService;
        private readonly IVideoHelper _videoHelper;
        private readonly IVideoConverterLogService _videoConverterLogService;

        public VideoHandler(
            IMediaService mediaService,
            IVideoHelper videoHelper,
            IVideoConverterLogService videoConverterLogService)
        {
            _mediaService = mediaService;
            _videoHelper = videoHelper;
            _videoConverterLogService = videoConverterLogService;
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
                    media.SetValue(Path.GetFileName(command.ConvertedFilePath), ms);
                }
            }

            System.IO.File.Delete(command.ConvertedFilePath);

            SaveVideoAdditionProperties(media);

            _mediaService.Save(media);

            _videoConverterLogService.Log(true, "Converted successfully", command.MediaId);

            return BroadcastResult.Success;
        }

        private void SaveVideoAdditionProperties(IMedia media)
        {
            var thumbnailUrl = _videoHelper.CreateThumbnail(media);
            media.SetValue(UmbracoAliases.Video.ThumbnailUrlPropertyAlias, thumbnailUrl);

            var videoSizeMetadata = _videoHelper.GetSizeMetadata(media);
            media.SetValue(UmbracoAliases.Video.VideoHeightPropertyAlias, videoSizeMetadata.Height);
            media.SetValue(UmbracoAliases.Video.VideoWidthPropertyAlias, videoSizeMetadata.Width);
        }
    }
}
