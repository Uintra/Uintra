using System.Configuration;

namespace uIntra.Core.LinkPreview
{
    public class LinkPreviewConfiguration : ILinkPreviewConfiguration
    {
        private const string ConfigKey = "linkPreviewServiceUri";
        public string LinkPreviewServiceUrl => ConfigurationManager.AppSettings[ConfigKey];
    }
}