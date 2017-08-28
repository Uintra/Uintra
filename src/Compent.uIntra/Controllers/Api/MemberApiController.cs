using uIntra.Users;
using uIntra.Users.Web;

namespace Compent.uIntra.Controllers.Api
{
    public class MemberApiController : MemberApiControllerBase
    {
        public MemberApiController(ICacheableIntranetUserService cacheableIntranetUserService) : base(cacheableIntranetUserService)
        {
        }
    }
}