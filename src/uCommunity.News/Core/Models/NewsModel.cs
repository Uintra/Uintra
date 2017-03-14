using System;
using uCommunity.Core.User;

namespace uCommunity.News
{
    public class NewsModel : NewsModelBase
    {
        public Guid? CreatorId { get; set; }
        public IntranetUserBase Creator { get; set; }
        public string OverviewPageUrl { get; set; }
        public string EditPageUrl { get; set; }
    }
}