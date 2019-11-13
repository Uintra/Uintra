using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Activity;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Core.LinkPreview;
using Uintra20.Core.Links;

namespace Uintra20.Core.Bulletins
{
    public class BulletinItemViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public LightboxGalleryPreviewModel LightboxGalleryPreviewInfo { get; set; }

        public IEnumerable<int> MediaIds { get; set; } = Enumerable.Empty<int>();

        public IntranetActivityItemHeaderViewModel HeaderInfo { get; set; }

        public Enum ActivityType { get; set; }

        public IActivityLinks Links { get; set; }

        public LinkPreviewViewModel LinkPreview { get; set; }

        public bool IsReadOnly { get; set; }
    }
}