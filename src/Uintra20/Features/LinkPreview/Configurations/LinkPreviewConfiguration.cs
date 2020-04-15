using Compent.LinkPreview.HttpClient;
using System;
using System.Configuration;

namespace Uintra20.Features.LinkPreview.Configurations
{
    public class LinkPreviewConfiguration : ILinkPreviewConfiguration
    {
        private const string SectionName = "linkPreviewServiceUri";

        public Uri ServiceUri => new Uri(ConfigurationManager.AppSettings[SectionName]);
    }
}