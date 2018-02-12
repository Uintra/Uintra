using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Activity;
using Uintra.Core.Controls.LightboxGallery;
using Uintra.Core.Links;
using Uintra.Core.Location;
using Uintra.Core.TypeProviders;

namespace Uintra.Events
{
    public class EventItemViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public LightboxGalleryPreviewModel LightboxGalleryPreviewInfo { get; set; }

        public IEnumerable<int> MediaIds { get; set; } = Enumerable.Empty<int>();

        public bool CanSubscribe { get; set; }

        public IntranetActivityItemHeaderViewModel HeaderInfo { get; set; }

        public bool IsPinned { get; set; }

        public bool IsPinActual { get; set; }

        public Enum ActivityType { get; set; }

        public IActivityLinks Links { get; set; }

        public bool IsReadOnly { get; set; }

        public string LocationTitle { get; set; }

        public ActivityLocation Location { get; set; }
    }
}