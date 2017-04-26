using System;

namespace uCommunity.Events.Dashboard
{
    public class EventBackofficeSaveModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Teaser { get; set; }
        public string Description { get; set; }
        public string Media { get; set; }
        public int UmbracoCreatorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsHidden { get; set; }
    }
}