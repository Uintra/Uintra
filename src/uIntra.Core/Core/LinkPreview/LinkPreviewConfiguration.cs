using System;
using System.Configuration;
using Compent.LinkPreview.Client;

namespace uIntra.Core.LinkPreview
{
    public class LinkPreviewConfiguration : ILinkPreviewConfiguration
    {
        private const string SectionName = "linkDetectionConfiguration";

        public Uri ServiceUri => new Uri(ConfigurationManager.AppSettings[""]);

        private class LinkPreviewConfig
        {
            
        }
    }
}