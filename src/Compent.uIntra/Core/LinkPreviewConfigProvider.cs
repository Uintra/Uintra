using uIntra.Core;

namespace Compent.uIntra.Core
{
    public class LinkPreviewConfigProvider : ILinkPreviewConfigProvider
    {
        public FooBananaConfig Config { get; } = new FooBananaConfig(new[] {@"https?:\/\/[^\s]+$"});
    }
}