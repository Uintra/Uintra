using uCommunity.Likes;
using uCommunity.News;

namespace Compent.uCommunity.Core.News.Models
{
    public class NewsExtendedItemViewModel : NewsItemViewModel
    {
        public ILikeable LikesInfo { get; set; }
    }
}