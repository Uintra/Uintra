using System;

namespace uIntra.Events.Dashboard
{
    public class EventBackofficeViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Media { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsHidden { get; set; }
        public DateTime ModifyDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}