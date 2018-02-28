using System;
using System.Configuration;
using Compent.LinkPreview.HttpClient;

namespace Uintra.Core.LinkPreview
{
    public class LinkPreviewConfiguration : ILinkPreviewConfiguration
    {
        private const string SectionName = "linkPreviewServiceUri";

        public Uri ServiceUri => new Uri(ConfigurationManager.AppSettings[SectionName]);
    }
}