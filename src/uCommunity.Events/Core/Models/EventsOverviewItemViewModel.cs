using System;
using System.Collections.Generic;
using System.Linq;
using uCommunity.Core.Activity.Models;
using uCommunity.Core.Controls.LightboxGallery;

namespace uCommunity.Events
{
    public class EventsOverviewItemViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Teaser { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public LightboxGalleryPreviewModel LightboxGalleryPreviewInfo { get; set; }

        public IEnumerable<int> MediaIds { get; set; } = Enumerable.Empty<int>();

        public bool CanSubscribe { get; set; }

        public IntranetActivityItemHeaderViewModel HeaderInfo { get; set; }

        public bool IsPinned { get; set; }        
    }
}