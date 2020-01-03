using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Activity.Models;
using Uintra20.Features.LinkPreview.Models;

namespace Uintra20.Features.Bulletins.Models
{
    public class SocialViewModel : IntranetActivityViewModelBase
    {
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public LinkPreviewViewModel LinkPreview { get; set; }
        public IEnumerable<string> Media { get; set; } = Enumerable.Empty<string>();
        public IEnumerable<SocialMedia> Files { get; set; } = Enumerable.Empty<SocialMedia>();
        public IEnumerable<SocialMedia> Images { get; set; } = Enumerable.Empty<SocialMedia>();

    }

    public class SocialMedia
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public string NameAndExtension
        {
            get => $"{Name}.{Extension}";
            set => value = $"{Name}.{Extension}";
        }
        public string Link { get; set; }

    }
}