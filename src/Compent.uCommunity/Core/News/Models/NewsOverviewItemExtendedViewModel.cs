using uCommunity.Likes;
using uCommunity.News;

namespace Compent.uCommunity.Core.News.Models
{
    public class NewsOverviewItemExtendedViewModel : NewsItemViewModel
    {
        public ILikeable LikesInfo { get; set; }
    }
}