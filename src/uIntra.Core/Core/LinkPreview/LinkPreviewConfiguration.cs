using System;
using System.Configuration;
using Compent.LinkPreview.Client;

namespace uIntra.Core.LinkPreview
{
    public class LinkPreviewConfiguration : ILinkPreviewConfiguration
    {
        private const string ConfigKey = "linkPreviewServiceUri";
        public Uri ServiceUri => new Uri(ConfigurationManager.AppSettings[ConfigKey]);
    }
}