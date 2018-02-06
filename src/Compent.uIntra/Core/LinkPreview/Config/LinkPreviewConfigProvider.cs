using System.Linq;
using uIntra.Core;
using uIntra.Core.LinkPreview;

namespace Compent.uIntra.Core.LinkPreview.Config
{
    public class LinkPreviewConfigProvider : ILinkPreviewConfigProvider
    {
        private readonly LinkDetectionConfigurationSection _section;

        public LinkPreviewConfigProvider()
        {
            _section = LinkDetectionConfigurationSection.Configuration;
        }

        public LinkDetectionConfig Config => 
            new LinkDetectionConfig { UrlRegex = _section.Regexes.Select(r => r.Key) };
    }
}