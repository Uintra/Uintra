using System;
using System.Collections.Generic;
using Uintra.Attributes;
using Uintra.Core.Activity.Models;
using Uintra.Features.LinkPreview.Models;
using Uintra.Features.Media;
using Uintra.Features.Media.Contracts;

namespace Uintra.Features.Social.Models
{
    public class SocialEditModel : IntranetActivityEditModelBase, IContentWithMediaCreateEditModel
    {
        [RequiredVirtual(IsRequired = false)]
        public override string Title { get; set; }

        [RequiredIfAllEmpty(DependancyProperties = new[] { nameof(NewMedia), nameof(Media) }), StringLengthWithoutHtml(2000)]
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public string Media { get; set; }

        [RequiredIfAllEmpty(DependancyProperties = new[] { nameof(Description), nameof(Media) })]
        public string NewMedia { get; set; }
        public LinkPreviewModel LinkPreview { get; set; }
        public int? LinkPreviewId { get; set; }
        public bool CanDelete { get; set; }
        public IEnumerable<Guid> TagIdsData { get; set; }
    }
}