using System.Linq;
using Uintra.Features.LinkPreview.Configurations;
using Uintra.Features.LinkPreview.Providers.Contracts;

namespace Uintra.Features.LinkPreview.Providers.Implementations
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