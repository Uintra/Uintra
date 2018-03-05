using System;
using System.Collections.Generic;

namespace Uintra.Core.LinkPreview
{
    [Serializable]
    public class LinkDetectionConfig
    {
        public IEnumerable<string> UrlRegex { get; set; }
    }
}