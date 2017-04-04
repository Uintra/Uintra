using System;
using uCommunity.Core.Activity.Models;

namespace uCommunity.News
{
    public class NewsOverviewItemModelBase
    {
        public Guid Id { get; set; }

        public string Teaser { get; set; }

        public DateTime PublishDate { get; set; }

        public string MediaIds { get; set; }

        public IntranetActivityHeaderModel HeaderInfo { get; set; }
    }
}