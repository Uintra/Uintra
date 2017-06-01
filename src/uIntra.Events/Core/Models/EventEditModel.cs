using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using uIntra.Core.Activity;
using uIntra.Core.Media;
using uIntra.Core.ModelBinders;
using uIntra.Core.User;
using uIntra.Events.Attributes;

namespace uIntra.Events
{
    public class EventEditModel : IntranetActivityEditModelBase, IContentWithMediaCreateEditModel, ICanEditCreatorCreateEditModel
    {
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

        [Required]
        public Guid CreatorId { get; set; }

        public IIntranetUser Creator { get; set; }

        public IEnumerable<IIntranetUser> Users { get; set; }

        public bool CanEditCreator { get; set; }


        public EventEditModel()
        {
            Users = Enumerable.Empty<IIntranetUser>();
        }
    }
}