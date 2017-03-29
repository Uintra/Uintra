using System;
using uCommunity.Core.User;

namespace uCommunity.News
{
    public class NewsOverviewItemModelBase
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Teaser { get; set; }

        public DateTime PublishDate { get; set; }

        public IIntranetUser Creator { get; set; }

        public string MediaIds { get; set; }
    }
}