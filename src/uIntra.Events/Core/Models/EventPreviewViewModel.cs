using System;
using Uintra.Core.Links;
using Uintra.Core.TypeProviders;
using Uintra.Core.User;

namespace Uintra.Events
{
    public class EventPreviewViewModel
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IIntranetUser Owner { get; set; }
        public Enum ActivityType { get; set; }
        public Guid Id { get; set; }
        public ActivityLinks Links { get; set; }
    }
}