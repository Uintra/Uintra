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
    public class EventCreateModel : IntranetActivityCreateModelBase, IContentWithMediaCreateEditModel, ICanEditCreatorCreateEditModel
    {
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

        [Required]
        public Guid CreatorId { get; set; }

        public IIntranetUser Creator { get; set; }

        public IEnumerable<IIntranetUser> Users { get; set; }

        public bool CanEditCreator { get; set; }

        public EventCreateModel()
        {
            Users = Enumerable.Empty<IIntranetUser>();
        }
    }
}