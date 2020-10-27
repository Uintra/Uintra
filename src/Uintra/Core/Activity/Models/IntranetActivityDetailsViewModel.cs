using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Controls.LightboxGallery;
using Uintra.Core.Member.Models;
using Uintra.Features.Links.Models;
using Uintra.Features.Location.Models;

namespace Uintra.Core.Activity.Models
{
    public class IntranetActivityDetailsViewModel : IHaveLightboxPreview
    {
        public Guid Id { get; set; }
        public bool CanEdit { get; set; }
        public bool IsPinned { get; set; }
        public string Title { get; set; }
        public MemberViewModel Owner { get; set; }
        public IEnumerable<string> Dates { get; set; } = Enumerable.Empty<string>();
        public IActivityLinks Links { get; set; }
        public ActivityLocation Location { get; set; }
        public bool IsReadOnly { get; set; }
        public string Type { get; set; }
        public Enum ActivityType { get; set; }
        public LightboxPreviewModel MediaPreview { get; set; }
        public string Description { get; set; }
    }
}