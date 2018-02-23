using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Uintra.Core.Activity;
using Uintra.Core.Attributes;
using Uintra.Core.Media;
using Uintra.Core.ModelBinders;

namespace Uintra.Bulletins
{
    public class BulletinEditModel : IntranetActivityEditModelBase, IContentWithMediaCreateEditModel
    {
        [RequiredVirtual(IsRequired = false)]
        public override string Title { get; set; }

        [RequiredIfAllEmpty(DependancyProperties = new[] { nameof(NewMedia), nameof(Media) }), AllowHtml, StringLength(2000)]
        public string Description { get; set; }

        [PropertyBinder(typeof(DateTimeBinder))]
        public DateTime PublishDate { get; set; }

        public string Media { get; set; }

        [Required]
        public int? MediaRootId { get; set; }

        [RequiredIfAllEmpty(DependancyProperties = new[] { nameof(Description), nameof(Media) })]
        public string NewMedia { get; set; }

        public int? LinkPreviewId { get; set; }
    }
}