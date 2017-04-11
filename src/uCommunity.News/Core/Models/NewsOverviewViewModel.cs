using System.Collections.Generic;
using System.Linq;

namespace uCommunity.News
{
    public class NewsOverviewViewModel
    {
        public IEnumerable<NewsOverviewItemViewModel> Items { get; set; }

        public string CreatePageUrl { get; set; }

        public string DetailsPageUrl { get; set; }

        public NewsOverviewViewModel()
        {
            Items = Enumerable.Empty<NewsOverviewItemViewModel>();
        }
    }
}