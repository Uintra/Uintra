using uIntra.Core.LinkPreview;

namespace uIntra.Core
{
    public interface ILinkPreviewConfigProvider
    {
        LinkDetectionConfig Config { get; }
    }
}
