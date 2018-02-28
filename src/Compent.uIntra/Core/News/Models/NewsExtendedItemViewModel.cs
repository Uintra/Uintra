using Compent.Uintra.Core.Activity.Models;
using Uintra.Likes;
using Uintra.News;

namespace Compent.Uintra.Core.News.Models
{
    public class NewsExtendedItemViewModel : NewsItemViewModel
    {
        public ILikeable LikesInfo { get; set; }
        public new ExtendedItemHeaderViewModel HeaderInfo { get; set; }
    }
}