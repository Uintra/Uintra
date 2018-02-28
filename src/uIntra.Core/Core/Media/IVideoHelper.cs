using Umbraco.Core.Models;

namespace UIntra.Core.Media
{
    public interface IVideoHelper
    {
        bool IsVideo(string fileExtension);
        string CreateThumbnail(IMedia media);
        VideoSizeMetadataModel GetSizeMetadata(IMedia media);
    }
}
