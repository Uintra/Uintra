using System;

namespace uIntra.Events.Dashboard
{
    public class EventBackofficeSaveModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Media { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime PublishDate { get; set; }
        public bool IsHidden { get; set; }
        public string LocationTitle { get; set; }
        public string LocationAddress { get; set; }
    }
}