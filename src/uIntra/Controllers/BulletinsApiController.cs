using uIntra.Bulletins;
using uIntra.Bulletins.Web;
using uIntra.Core.Bulletins;
using uIntra.Core.Media;
using uIntra.Core.User;
using uIntra.Users;

namespace uIntra.Controllers
{
    public class BulletinsApiController : BulletinsBaseApiController
    {
        public BulletinsApiController(
            IIntranetUserService<IntranetUser> intranetUserService,
            IBulletinsService<Bulletin> bulletinService, 
            IMediaHelper mediaHelper)
            : base(intranetUserService, bulletinService, mediaHelper)
        {
        }
    }
}