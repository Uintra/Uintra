using System;
using uIntra.Core.Activity.Models;

namespace uCommunity.News
{
    public class NewsViewModel : IntranetActivityViewModelBase
    {
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public string Media { get; set; }
    }
}