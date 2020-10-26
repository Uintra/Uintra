using System;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Member.Abstractions;

namespace Uintra20.Features.Events
{
    public class EventBase : IntranetActivity, IHaveCreator, IHaveOwner
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime PublishDate { get; set; }
        public Guid CreatorId { get; set; }
        public Guid OwnerId { get; set; }
        public int? UmbracoCreatorId { get; set; }
        public string LocationTitle { get; set; }
    }
}