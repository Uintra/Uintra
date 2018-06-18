using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Activity;
using Uintra.Core.Controls.LightboxGallery;
using Uintra.Core.Links;

namespace Uintra.Core.PagePromotion
{
    public class PagePromotionItemViewModel
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public DateTime PublishDate { get; set; }

        public LightboxGalleryPreviewModel LightboxGalleryPreviewInfo { get; set; }

        public IEnumerable<int> MediaIds { get; set; } = Enumerable.Empty<int>();

        public IntranetActivityItemHeaderViewModel HeaderInfo { get; set; }

        public Enum ActivityType { get; set; }

        public string Url { get; set; }

        public IActivityLinks Links { get; set; }

        public bool IsReadOnly { get; set; }
    }
}
