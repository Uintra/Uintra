using System;
using System.Collections.Generic;
using System.Linq;
using uCommunity.Core.Activity.Models;
using uCommunity.Core.Controls.LightboxGallery;

namespace uCommunity.News
{
    public class NewsItemViewModel
    {
        public Guid Id { get; set; }

        public string ShortDescription { get; set; }

        public DateTime PublishDate { get; set; }

        public bool Expired { get; set; }

        public LightboxGalleryPreviewModel LightboxGalleryPreviewInfo { get; set; }

        public IEnumerable<int> MediaIds { get; set; } = Enumerable.Empty<int>();

        public IntranetActivityItemHeaderViewModel HeaderInfo { get; set; }

        public bool IsPinned { get; set; }        
    }
}