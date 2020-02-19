using System;
using System.ComponentModel.DataAnnotations;
using Uintra20.Attributes;
using Uintra20.Core.Activity.Models;
using Uintra20.Features.LinkPreview.Models;
using Uintra20.Features.Media;

namespace Uintra20.Features.Social.Edit.Models
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
        public LinkPreviewViewModel LinkPreview { get; set; }
        public int? LinkPreviewId { get; set; }
        public bool CanDelete { get; set; }
    }
}