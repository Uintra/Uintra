using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Uintra.Attributes;
using Uintra.Core.Activity.Models;
using Uintra.Features.Media;
using Uintra.Features.Media.Contracts;

namespace Uintra.Features.Events.Models
{
    public class EventEditModel : IntranetActivityEditModelBase, IContentWithMediaCreateEditModel
    {
        [Required, AllowHtml]
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        [GreaterThan("StartDate")]
        public DateTime EndDate { get; set; }
        public DateTime PublishDate { get; set; }
        public string Media { get; set; }
        public string NewMedia { get; set; }
        public bool NotifyAllSubscribers { get; set; }
        [RequiredIf("IsPinned", true), GreaterThan("PublishDate")]
        public override DateTime? EndPinDate { get; set; }
        public string LocationTitle { get; set; }
        public bool PinAllowed { get; set; }
        public bool CanSubscribe { get; set; }
        public string SubscribeNotes { get; set; }
        public IEnumerable<Guid> TagIdsData { get; set; }
    }
}