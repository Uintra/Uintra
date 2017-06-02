using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using uIntra.Core.Activity;
using uIntra.Core.Controls.LightboxGallery;

namespace uIntra.Events
{
    public class EventItemViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public LightboxGalleryPreviewModel LightboxGalleryPreviewInfo { get; set; }

        public IEnumerable<int> MediaIds { get; set; } = Enumerable.Empty<int>();

        public bool CanSubscribe { get; set; }

        public IntranetActivityItemHeaderViewModel HeaderInfo { get; set; }

        public bool IsPinned { get; set; }        
    }
}