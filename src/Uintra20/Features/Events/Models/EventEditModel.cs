using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Uintra20.Attributes;
using Uintra20.Core.Activity.Models;
using Uintra20.Features.Media;

namespace Uintra20.Features.Events.Models
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
        public bool CanEditSubscribe { get; set; }
        public string TagIdsData { get; set; }
    }
}