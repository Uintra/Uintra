using Uintra.Users;
using Uintra.Users.Web;

namespace Compent.Uintra.Controllers.Api
{
    public class MemberApiController : MemberApiControllerBase
    {
        public MemberApiController(ICacheableIntranetUserService cacheableIntranetUserService) : base(cacheableIntranetUserService)
        {
        }
    }
}