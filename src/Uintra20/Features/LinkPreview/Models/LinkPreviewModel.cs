﻿using System;

namespace Uintra20.Features.LinkPreview.Models
{
    public class LinkPreviewModel
    {
        public int Id { get; set; }
        public string Uri { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Uri ImageUri { get; set; }
        public Uri FaviconUri { get; set; }
    }
}