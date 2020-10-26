using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Controls.LightboxGallery;

namespace Uintra.Features.Groups.Models
{
    public class GroupInfoViewModel : IHaveLightboxPreview
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Media { get; set; } = Enumerable.Empty<string>();
        public LightboxPreviewModel MediaPreview { get; set; }
        public bool CanHide { get; set; }
        public bool IsHidden { get; set; }
    }
}