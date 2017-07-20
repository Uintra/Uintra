using System;
using uIntra.Core.User;

namespace Compent.uIntra.Core.Events
{
    public class EventPreviewViewModel
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IIntranetUser Creator { get; set; }
    }
}