using System;
using System.Collections.Generic;

namespace Uintra20.Features.LinkPreview.Configurations
{
    [Serializable]
    public class LinkDetectionConfig
    {
        public IEnumerable<string> UrlRegex { get; set; }
    }
}