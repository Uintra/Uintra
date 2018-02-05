using Umbraco.Core.Models;

namespace uIntra.Core.Media
{
    public interface IVideoHelper
    {
        bool IsVideo(string fileExtension);
        string CreateThumbnail(IMedia media);
        VideoSizeMetadataModel GetSizeMetadata(IMedia media);
    }
}
