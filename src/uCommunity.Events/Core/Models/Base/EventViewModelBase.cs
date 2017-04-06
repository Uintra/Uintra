using System;
using uCommunity.Core.Activity.Models;

namespace uCommunity.Events
{
    public class EventViewModelBase : IntranetActivityViewModelBase//IntranetActivityModelBase
    {
        public Guid? CreatorId { get; set; }
        public string OverviewPageUrl { get; set; }
        public string EditPageUrl { get; set; }

        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Media { get; set; }
        public bool CanSubscribe { get; set; }
    }
}