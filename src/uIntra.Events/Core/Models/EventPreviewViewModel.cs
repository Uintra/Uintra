using System;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;

namespace uIntra.Events
{
    public class EventPreviewViewModel
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IIntranetUser Creator { get; set; }
        public IIntranetType ActivityType { get; set; }
        public Guid Id { get; set; }
    }
}