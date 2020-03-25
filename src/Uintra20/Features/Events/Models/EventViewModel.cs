using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Activity.Models;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Features.Tagging.UserTags.Models;

namespace Uintra20.Features.Events.Models
{
    public class EventViewModel : IntranetActivityViewModelBase
    {
        public Guid? CreatorId { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public string StartDateString { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? EndPinDate { get; set; }
        public string EndDateString { get; set; }
        public DateTime PublishDate { get; set; }
        public IEnumerable<string> FullEventTime { get; set; }
        public int EventDate { get; set; }
        public string EventMonth { get; set; }
        public IEnumerable<string> Media { get; set; }
        public bool CanSubscribe { get; set; }
        public string SubscribeNotes { get; set; }
        public string LocationTitle { get; set; }
        public LightboxPreviewModel LightboxPreviewModel { get; set; }
        public IEnumerable<UserTag> Tags { get; set; } = Enumerable.Empty<UserTag>();
        public IEnumerable<UserTag> AvailableTags { get; set; } = Enumerable.Empty<UserTag>();
        public bool IsSubscribed { get; set; }
        public bool IsNotificationsDisabled { get; set; }
    }
}