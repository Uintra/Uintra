using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Uintra.Core.Activity;
using Uintra.Core.Attributes;
using Uintra.Core.Location;
using Uintra.Core.Media;
using Uintra.Core.ModelBinders;

namespace Uintra.News
{
    public class NewsEditModel : IntranetActivityEditModelBase, IContentWithMediaCreateEditModel
    {
        [Required, AllowHtml]
        public string Description { get; set; }

        [PropertyBinder(typeof(DateTimeBinder))]
        public DateTime PublishDate { get; set; }

        [PropertyBinder(typeof(DateTimeBinder))]
        public DateTime? UnpublishDate { get; set; }

        public string Media { get; set; }

        public int? MediaRootId { get; set; }

        public string NewMedia { get; set; }

        [RequiredIf("IsPinned", true), GreaterThan("PublishDate")]
        public override DateTime? EndPinDate { get; set; }
    }
}