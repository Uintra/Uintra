using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Activity;
using uIntra.Core.Controls.LightboxGallery;

namespace uIntra.Bulletins
{
    public class BulletinItemViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public LightboxGalleryPreviewModel LightboxGalleryPreviewInfo { get; set; }

        public IEnumerable<int> MediaIds { get; set; } = Enumerable.Empty<int>();

        public IntranetActivityItemHeaderViewModel HeaderInfo { get; set; }
    }
}
