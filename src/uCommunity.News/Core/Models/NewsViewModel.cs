using System;
using uCommunity.Core.User;

namespace uCommunity.News
{
    public class NewsViewModel : NewsViewModelBase
    {
        public Guid? CreatorId { get; set; }
        public IntranetUserBase Creator { get; set; }
        public string OverviewPageUrl { get; set; }
        public string EditPageUrl { get; set; }
    }
}