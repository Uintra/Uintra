using Uintra20.Features.Media.Video.Models;
using Umbraco.Core.Models;

namespace Uintra20.Features.Media.Video.Helpers.Contracts
{
    public interface IVideoHelper
    {
        bool IsVideo(string fileExtension);
        string CreateThumbnail(IMedia media);
        VideoSizeMetadataModel GetSizeMetadata(IMedia media);
        string CreateConvertingThumbnail();
        string CreateConvertingFailureThumbnail();
    }
}
