using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Uintra.Core.Activity;
using Uintra.Core.Attributes;
using Uintra.Core.Location;
using Uintra.Core.Media;
using Uintra.Core.ModelBinders;

namespace Uintra.Events
{
    public class EventEditModel : IntranetActivityEditModelBase, IContentWithMediaCreateEditModel
    {
        [Required, AllowHtml]
        public string Description { get; set; }

        [PropertyBinder(typeof(DateTimeBinder))]
        public DateTime StartDate { get; set; }

        [GreaterThan("StartDate"), PropertyBinder(typeof(DateTimeBinder))]
        public DateTime EndDate { get; set; }

        [PropertyBinder(typeof(DateTimeBinder))]
        public DateTime PublishDate { get; set; }

        public string Media { get; set; }

        public string NewMedia { get; set; }

        public int? MediaRootId { get; set; }

        public bool NotifyAllSubscribers { get; set; }

        [RequiredIf("IsPinned", true), GreaterThan("PublishDate")]
        public override DateTime? EndPinDate { get; set; }

        public string LocationTitle { get; set; }

        public bool CanHide { get; set; }

        public bool PinAllowed { get; set; }
	}
}