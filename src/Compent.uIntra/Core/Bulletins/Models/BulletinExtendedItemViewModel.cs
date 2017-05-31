using uIntra.Bulletins;
using uIntra.Likes;

namespace Compent.uIntra.Core.Bulletins
{
    public class BulletinExtendedItemViewModel : BulletinItemViewModel
    {
        public ILikeable LikesInfo { get; set; }
    }
}