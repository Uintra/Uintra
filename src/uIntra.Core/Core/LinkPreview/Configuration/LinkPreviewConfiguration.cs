﻿using System;
using System.Configuration;
using Compent.LinkPreview.Client;

namespace uIntra.Core.LinkPreview
{
    public class LinkPreviewConfiguration : ILinkPreviewConfiguration
    {
        private const string SectionName = "linkPreviewServiceUri";

        public Uri ServiceUri => new Uri(ConfigurationManager.AppSettings[SectionName]);
    }
}