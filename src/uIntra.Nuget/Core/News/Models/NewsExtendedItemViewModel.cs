using uIntra.Likes;
using uIntra.News;

namespace uIntra.Core.News.Models
{
    public class NewsExtendedItemViewModel : NewsItemViewModel
    {
        public ILikeable LikesInfo { get; set; }
    }
}