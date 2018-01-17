using System;

namespace uIntra.Core.LinkPreview
{
    public class LinkPreviewViewModel
    {
        public Uri Uri { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Uri ImageUri { get; set; }
        public Uri FaviconUri { get; set; }
    }
}
