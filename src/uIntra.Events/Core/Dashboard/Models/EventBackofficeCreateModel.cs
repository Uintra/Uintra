using System;

namespace uIntra.Events.Dashboard
{
    public class EventBackofficeCreateModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Media { get; set; }
        public int? UmbracoCreatorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsHidden { get; set; }
    }
}