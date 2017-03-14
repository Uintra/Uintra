using System.Collections.Generic;
using System.Linq;

namespace uCommunity.News
{
    public class NewsOverviewModel
    {
        public IEnumerable<NewsOverviewItemModel> Items { get; set; }

        public string CreatePageUrl { get; set; }

        public string DetailsPageUrl { get; set; }

        public NewsOverviewModel()
        {
            Items = Enumerable.Empty<NewsOverviewItemModel>();
        }
    }
}