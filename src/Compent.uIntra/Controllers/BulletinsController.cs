using System.Linq;
using System.Web.Mvc;
using Compent.uIntra.Core.Bulletins;
using uIntra.Bulletins;
using uIntra.Bulletins.Web;
using uIntra.CentralFeed;
using uIntra.Core.Extentions;
using uIntra.Core.Media;
using uIntra.Core.User;

namespace Compent.uIntra.Controllers
{
    public class BulletinsController : BulletinsControllerBase
    {
        public BulletinsController(
            IBulletinsService<Bulletin> bulletinsService,
            IMediaHelper mediaHelper,
            IIntranetUserService<IIntranetUser> intranetUserService)
            : base(bulletinsService, mediaHelper, intranetUserService)
        {
        }

        public ActionResult CentralFeedItem(ICentralFeedItem item)
        {
            FillLinks();

            var activity = item as Bulletin;
            var extendedModel = GetItemViewModel(activity).Map<BulletinExtendedItemViewModel>();
            extendedModel.LikesInfo = activity;
            return PartialView(ItemViewPath, extendedModel);
        }

    }
}