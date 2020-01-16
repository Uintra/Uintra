using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Activity.Models;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Features.LinkPreview.Models;

namespace Uintra20.Features.Social.Models
{
    public class SocialViewModel : IntranetActivityViewModelBase
    {
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public LinkPreviewViewModel LinkPreview { get; set; }
        public IEnumerable<string> Media { get; set; } = Enumerable.Empty<string>();
        public LightboxPreviewModel LightboxPreviewModel { get; set; }
    }
}