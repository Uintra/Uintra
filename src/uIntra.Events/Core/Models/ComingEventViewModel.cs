using System;
using uIntra.Core.Links;
using uIntra.Core.User;

namespace uIntra.Events
{
    public class ComingEventViewModel
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IIntranetUser Owner { get; set; }
        public IActivityLinks Links { get; set; }
    }
}