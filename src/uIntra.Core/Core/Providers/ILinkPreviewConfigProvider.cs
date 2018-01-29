using System;
using System.Collections.Generic;

namespace uIntra.Core
{
    public interface ILinkPreviewConfigProvider
    {
        FooBananaConfig Config { get; }
    }

    [Serializable]
    public class FooBananaConfig
    {
        public IEnumerable<string> UrlRegex { get; set; }

        public FooBananaConfig(IEnumerable<string> urlRegex)
        {
            UrlRegex = urlRegex;
        }
    }
}
