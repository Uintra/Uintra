using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using uIntra.Core.Activity;
using uIntra.Core.Attributes;
using uIntra.Core.Media;
using uIntra.Core.ModelBinders;

namespace uIntra.Events
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

        public bool CanSubscribe { get; set; }

        public bool CanEditSubscribe { get; set; }

        public int? MediaRootId { get; set; }

        public bool NotifyAllSubscribers { get; set; }

        [RequiredIf("IsPinned", true), GreaterThan("PublishDate")]
        public override DateTime? EndPinDate { get; set; }

        public string LocationTitle { get; set; }

        public string LocationAddress { get; set; }
    }
}