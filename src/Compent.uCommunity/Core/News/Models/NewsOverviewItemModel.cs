using uCommunity.Likes;
using uCommunity.News;

namespace Compent.uCommunity.Core.News.Models
{
    public class NewsOverviewItemModel : NewsOverviewItemModelBase 
    {
        public ILikeable LikesInfo { get; set; }
    }
}