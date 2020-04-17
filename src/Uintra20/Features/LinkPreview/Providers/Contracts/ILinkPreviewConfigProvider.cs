using Uintra20.Features.LinkPreview.Configurations;

namespace Uintra20.Features.LinkPreview.Providers.Contracts
{
    public interface ILinkPreviewConfigProvider
    {
        LinkDetectionConfig Config { get; }
    }
}