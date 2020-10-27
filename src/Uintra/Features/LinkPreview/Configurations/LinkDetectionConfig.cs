using System;
using System.Collections.Generic;

namespace Uintra.Features.LinkPreview.Configurations
{
    [Serializable]
    public class LinkDetectionConfig
    {
        public IEnumerable<string> UrlRegex { get; set; }
    }
}