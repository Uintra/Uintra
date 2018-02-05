using Umbraco.Core.Models;

namespace uIntra.Core.Media
{
    public interface IVideoHelper
    {
        bool IsVideo(IMedia media);
        bool IsVideo(string fileExtension);
        void CreateThumbnail(IMedia media);
        string GetThumbnail(IPublishedContent media);
        VideoSizeMetadataModel GetSizeMetadata(IMedia media);
    }
}
