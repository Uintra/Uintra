using Uintra.Core.LinkPreview;

namespace Uintra.Core
{
    public interface ILinkPreviewConfigProvider
    {
        LinkDetectionConfig Config { get; }
    }
}
