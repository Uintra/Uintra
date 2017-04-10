using System;
using uCommunity.Core.Activity.Models;

namespace uCommunity.News
{
    public class NewsViewModel : IntranetActivityViewModelBase
    {
        public string Teaser { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public string Media { get; set; }

        public string OverviewPageUrl { get; set; }
        public string EditPageUrl { get; set; }
    }
}