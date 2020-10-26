using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Activity.Models;
using Uintra.Core.Controls.LightboxGallery;
using Uintra.Features.LinkPreview.Models;

namespace Uintra.Features.Social.Models
{
    public class SocialViewModel : IntranetActivityViewModelBase
    {
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public LinkPreviewModel LinkPreview { get; set; }
        public IEnumerable<string> Media { get; set; } = Enumerable.Empty<string>();
        public LightboxPreviewModel LightboxPreviewModel { get; set; }
    }
}