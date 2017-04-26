using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using uCommunity.Core.Activity.Models;
using uCommunity.Core.Media;
using uCommunity.Core.ModelBinders;

namespace uCommunity.Events
{
    public class EventEditModel : IntranetActivityEditModelBase, IContentWithMediaCreateEditModel
    {
        [Required, StringLength(2000)]
        public string Teaser { get; set; }

        [Required, AllowHtml]
        public string Description { get; set; }

        [PropertyBinder(typeof(DateTimeBinder))]
        public DateTime StartDate { get; set; }

        [GreaterThan("StartDate"), PropertyBinder(typeof(DateTimeBinder))]
        public DateTime EndDate { get; set; }

        public string Media { get; set; }

        public string NewMedia { get; set; }

        public bool CanSubscribe { get; set; }

        public bool CanEditSubscribe { get; set; }

        public int? MediaRootId { get; set; }

        public bool NotifyAllSubscribers { get; set; }
    }
}