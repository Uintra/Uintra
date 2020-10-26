using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Activity.Models.Headers;
using Uintra.Core.Controls.LightboxGallery;
using Uintra.Features.LinkPreview.Models;
using Uintra.Features.Links.Models;

namespace Uintra.Features.Social.Models
{
    public class SocialItemViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public LightboxGalleryPreviewModel LightboxGalleryPreviewInfo { get; set; }

        public IEnumerable<int> MediaIds { get; set; } = Enumerable.Empty<int>();

        public IntranetActivityItemHeaderViewModel HeaderInfo { get; set; }

        public Enum ActivityType { get; set; }

        public IActivityLinks Links { get; set; }

        public LinkPreviewModel LinkPreview { get; set; }

        public bool IsReadOnly { get; set; }
    }
}