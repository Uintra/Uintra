using System.Collections.Generic;
using System.Linq;

namespace Uintra.News
{
    public class NewsOverviewViewModel
    {
        public IEnumerable<NewsItemViewModel> Items { get; set; }

        public string CreatePageUrl { get; set; }

        public string DetailsPageUrl { get; set; }

        public NewsOverviewViewModel()
        {
            Items = Enumerable.Empty<NewsItemViewModel>();
        }
    }
}