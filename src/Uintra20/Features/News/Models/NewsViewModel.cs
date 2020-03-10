using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Activity.Models;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Features.Tagging.UserTags.Models;

namespace Uintra20.Features.News.Models
{
    public class NewsViewModel : IntranetActivityViewModelBase
    {
        public Guid OwnerId { get; set; }
        public string Description { get; set; }
        public DateTime? EndPinDate { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime? UnpublishDate { get; set; }
        public IEnumerable<string> Media { get; set; } = Enumerable.Empty<string>();
        public LightboxPreviewModel LightboxPreviewModel { get; set; }
        public IEnumerable<UserTag> Tags { get; set; } = Enumerable.Empty<UserTag>();
        public IEnumerable<UserTag> AvailableTags { get; set; } = Enumerable.Empty<UserTag>();
    }
}