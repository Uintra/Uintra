using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Uintra20.Attributes;
using Uintra20.Core.Activity.Models;
using Uintra20.Features.Media;

namespace Uintra20.Features.Events.Models
{
    public class EventCreateModel :
        IntranetActivityCreateModelBase,
        IContentWithMediaCreateEditModel
    {
        [Required, AllowHtml]
        public string Description { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required, GreaterThan("StartDate")]
        public DateTime EndDate { get; set; }
        [Required]
        public DateTime PublishDate { get; set; }
        public string Media { get; set; }
        public string NewMedia { get; set; }
        [RequiredIf("IsPinned", true), GreaterThan("PublishDate")]
        public override DateTime? EndPinDate { get; set; }
        public string LocationTitle { get; set; }
        public bool PinAllowed { get; set; }
        public bool CanSubscribe { get; set; }
        public string SubscribeNotes { get; set; }
        public IEnumerable<Guid> TagIdsData { get; set; }
        public Guid? GroupId { get; set; }
    }
}