using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using uCommunity.Core.Activity.Models;
using uCommunity.Core.Media;
using uCommunity.Core.ModelBinders;

namespace uCommunity.Events
{
    public class EventCreateModel : IntranetActivityCreateModelBase, IContentWithMediaCreateEditModel
    {
        [Required, StringLength(2000)]
        public string Teaser { get; set; }

        [Required, AllowHtml]
        public string Description { get; set; }

        [Required, PropertyBinder(typeof(DateTimeBinder))]
        public DateTime StartDate { get; set; }

        [Required, GreaterThan("StartDate"), PropertyBinder(typeof(DateTimeBinder))]
        public DateTime EndDate { get; set; }

        public string Media { get; set; }

        public string NewMedia { get; set; }

        public bool CanSubscribe { get; set; }

        public int? MediaRootId { get; set; }        
    }
}