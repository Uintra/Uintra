using System;
using System.ComponentModel.DataAnnotations;
using Uintra20.Attributes;
using Uintra20.Core.Activity;
using Uintra20.Core.LinkPreview;
using Uintra20.Core.Media;

namespace Uintra20.Core.Bulletins
{
    public class BulletinEditModel : IntranetActivityEditModelBase, IContentWithMediaCreateEditModel
    {
        public override string Title { get; set; }

        [RequiredIfAllEmpty(DependancyProperties = new[] { nameof(NewMedia), nameof(Media) }), StringLengthWithoutHtml(2000)]
        public string Description { get; set; }

        public DateTime PublishDate { get; set; }

        public string Media { get; set; }

        [Required]
        public int? MediaRootId { get; set; }

        [RequiredIfAllEmpty(DependancyProperties = new[] { nameof(Description), nameof(Media) })]
        public string NewMedia { get; set; }

        public LinkPreviewViewModel LinkPreview { get; set; }
        public int? LinkPreviewId { get; set; }
        public bool CanDelete { get; set; }
    }
}