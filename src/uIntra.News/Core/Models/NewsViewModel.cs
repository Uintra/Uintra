using System;
using uIntra.Core.Activity;
using uIntra.Core.Location;

namespace uIntra.News
{
    public class NewsViewModel : IntranetActivityViewModelBase
    {
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public string Media { get; set; }
        public ActivityLocation Location { get; set; }
    }
}