using System.Linq;
using Uintra.Core;
using Uintra.Core.LinkPreview;

namespace Compent.Uintra.Core.LinkPreview.Config
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