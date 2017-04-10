using uCommunity.Likes;
using uCommunity.News;

namespace Compent.uCommunity.Core.News.Models
{
    public class NewsOverviewItemExtendedViewModel : NewsOverviewItemViewModel
    {
        public ILikeable LikesInfo { get; set; }
    }
}