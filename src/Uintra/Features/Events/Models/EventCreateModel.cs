﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Uintra.Attributes;
using Uintra.Core.Activity.Models;
using Uintra.Features.Media.Contracts;

namespace Uintra.Features.Events.Models
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
        [StringLength(200)]
        public string LocationTitle { get; set; }
        public bool PinAllowed { get; set; }
        public bool CanSubscribe { get; set; }
        public string SubscribeNotes { get; set; }
        public IEnumerable<Guid> TagIdsData { get; set; }
        public Guid? GroupId { get; set; }
    }
}