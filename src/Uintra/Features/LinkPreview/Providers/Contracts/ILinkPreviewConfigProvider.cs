using Uintra.Features.LinkPreview.Configurations;

namespace Uintra.Features.LinkPreview.Providers.Contracts
{
    public interface ILinkPreviewConfigProvider
    {
        LinkDetectionConfig Config { get; }
    }
}