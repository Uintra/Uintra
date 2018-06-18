using Compent.uIntra.Core.Activity.Models;
using uIntra.Likes;
using uIntra.News;

namespace Compent.uIntra.Core.News.Models
{
    public class NewsExtendedItemViewModel : NewsItemViewModel
    {
        public ILikeable LikesInfo { get; set; }
        public new ExtendedItemHeaderViewModel HeaderInfo { get; set; }
    }
}