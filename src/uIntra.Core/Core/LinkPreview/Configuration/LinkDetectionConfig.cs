using System;
using System.Collections.Generic;

namespace uIntra.Core.LinkPreview
{
    [Serializable]
    public class LinkDetectionConfig
    {
        public IEnumerable<string> UrlRegex { get; set; }
    }
}