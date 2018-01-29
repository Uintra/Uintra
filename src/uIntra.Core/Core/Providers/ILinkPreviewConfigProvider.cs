using System.Collections.Generic;

namespace uIntra.Core
{
    public interface ILinkPreviewConfigProvider
    {
        FooBananaConfig Config { get; }
    }

    public class FooBananaConfig
    {
        public IEnumerable<string> UrlRegex { get; }

        public FooBananaConfig(IEnumerable<string> urlRegex)
        {
            UrlRegex = urlRegex;
        }
    }
}
